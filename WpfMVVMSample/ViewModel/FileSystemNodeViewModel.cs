/*
 * FileSystemNodeViewModel.cs
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

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WpfMVVMSample.ICommandImpl;
using WpfMVVMSample.Model;

namespace WpfMVVMSample.ViewModel {

    internal class FileSystemNodeViewModel : INotifyPropertyChanged {
        private FileSystemNodeModel? model;

        private ObservableCollection<FileSystemNodeViewModel> children;

        private ObservableCollection<FileSystemNodeViewModel> files;

        private Action<bool> refreshParent;

        public FileSystemNodeModel Model => model!;

        public string Name => model!.Name;

        public string Path => model!.Path;

        public BitmapImage? Thumb => model!.Thumb;

        public ObservableCollection<FileSystemNodeViewModel> Children {
            set {
                children = value;
                NotifyPropertyChanged(nameof(Children));
            }
            get => children;
        }

        public ObservableCollection<FileSystemNodeViewModel> Files {
            set {
                files = value;
                NotifyPropertyChanged(nameof(Files));
            }
            get => files;
        }

        public bool IsExpanded {
            set {
                model!.IsExpanded = value;
                Describe(recursive: true);
            }
            get => model!.IsExpanded;
        }

        public bool IsSelected {
            set {
                model!.IsSelected = value;
                refreshParent(false);
            }
            get => model!.IsSelected;
        }

        public ICommand? ContextMenuDeleteCommand { get; set; }

        public FileSystemNodeViewModel(Action<bool> refreshParent) {
            children = new ObservableCollection<FileSystemNodeViewModel>();
            files = new ObservableCollection<FileSystemNodeViewModel>();
            this.refreshParent = refreshParent;
        }

        public void Init(string path) {
            model = new FileSystemNodeModel(path);
            ContextMenuDeleteCommand = new ContextMenuDeleteCommand(model.CanDelete, RunDelete);
        }

        public void Describe(bool recursive = false, bool force = false) {
            if (model!.Described && !force) return;

            children = new ObservableCollection<FileSystemNodeViewModel>();
            files = new ObservableCollection<FileSystemNodeViewModel>();

            var subDirectories = model!.GetDirectories();
            foreach (var directory in subDirectories) {
                var addChild = new FileSystemNodeViewModel(refreshParent);
                addChild.Init(directory);
                if (recursive) {
                    addChild.Describe();
                }
                children.Add(addChild);
            }

            var subFiles = model!.GetFiles();
            foreach (var file in subFiles) {
                var addChild = new FileSystemNodeViewModel(refreshParent);
                addChild.Init(file);
                children.Add(addChild);
                files.Add(addChild);
            }

            model.Described = true;
            NotifyPropertyChanged(nameof(Children));
        }

        private void RunDelete() {
            model!.RunDelete();
            refreshParent(true);
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged(string name = @"") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        #endregion INotifyPropertyChanged
    }
}