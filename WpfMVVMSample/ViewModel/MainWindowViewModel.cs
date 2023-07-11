/*
 * MainWindowViewModel.cs
 * 
 * Copyright 2023 Yuichi Yoshii
 *     吉井雄一 @ 吉井産業  you.65535.kir@gmail.com
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using WpfMVVMSample.Model;

namespace WpfMVVMSample.ViewModel {

    internal class MainWindowViewModel : INotifyPropertyChanged {
        private MainWindowModel model;

        private ObservableCollection<FileSystemNodeViewModel> nodes;

        private ObservableCollection<FileSystemNodeViewModel> blankFileList = new ObservableCollection<FileSystemNodeViewModel>();

        public ObservableCollection<FileSystemNodeViewModel> Nodes {
            set {
                nodes = value;
                NotifyPropertyChanged(nameof(Nodes));
            }
            get => nodes;
        }

        public ObservableCollection<FileSystemNodeViewModel> Files {
            get {
                var s = GetSelectedNode(nodes);
                if (s == null) {
                    return blankFileList;
                }
                return s.Files;
            }
        }

        public int FontSize {
            set {
                model.FontSize = value;
                NotifyPropertyChanged(nameof(FontSize));
            }
            get => model.FontSize;
        }

        public int ThumbHeight {
            set {
                model.ThumbHeight = value;
                NotifyPropertyChanged(nameof(ThumbHeight));
                NotifyPropertyChanged(nameof(StackPanelItemHeight));
            }
            get => model.ThumbHeight;
        }

        public int StackPanelItemHeight => model.StackPanelItemHeight;

        public int ThumbWidth {
            set {
                model.ThumbWidth = value;
                NotifyPropertyChanged(nameof(ThumbWidth));
                NotifyPropertyChanged(nameof(StackPanelItemWidth));
            }
            get => model.ThumbWidth;
        }

        public int StackPanelItemWidth => model.StackPanelItemWidth;

        public MainWindowViewModel() {
            model = new MainWindowModel();
            NotifyPropertyChanged(nameof(ThumbHeight));
            NotifyPropertyChanged(nameof(StackPanelItemHeight));
            NotifyPropertyChanged(nameof(ThumbWidth));
            NotifyPropertyChanged(nameof(StackPanelItemWidth));
            nodes = new ObservableCollection<FileSystemNodeViewModel>();

            Fill();
        }

        private void Fill() {
            foreach (var d in DriveInfo.GetDrives()) {
                if (d.Name == @"N:\") continue;
                if (d.Name == @"P:\") continue;
                var add = new FileSystemNodeViewModel(Refresh);
                add.Init(d.Name);
                nodes.Add(add);
            }
            NotifyPropertyChanged(nameof(Nodes));
        }

        private FileSystemNodeViewModel? GetSelectedNode(IEnumerable<FileSystemNodeViewModel> nodes) {
            var selected = nodes.FirstOrDefault(n => n.IsSelected);
            if (selected != null) {
                return selected;
            }

            foreach (var node in nodes) {
                selected = GetSelectedNode(node.Children);
                if (selected != null) {
                    return selected;
                }
            }

            return null;
        }

        private void Refresh(bool reload) {
            if (reload) {
                var s = GetSelectedNode(nodes);
                s?.Describe(false, true);
            }
            NotifyPropertyChanged(nameof(Files));
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged(string name = @"") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        #endregion INotifyPropertyChanged
    }
}