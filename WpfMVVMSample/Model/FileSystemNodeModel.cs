/*
 * FileSystemNodeModel.cs
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

using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WpfMVVMSample.Model {

    internal class FileSystemNodeModel : INotifyPropertyChanged {
        private bool isExpanded;

        private bool isSelected;

        public string Name { set; get; } = string.Empty;

        public string Path { set; get; } = string.Empty;

        public BitmapImage? Thumb { set; get; }

        public bool Described { set; get; } = false;

        public bool IsDirectory { set; get; } = false;

        public bool IsExpanded {
            set {
                isExpanded = value;
                NotifyPropertyChanged(nameof(IsExpanded));
            }
            get => isExpanded;
        }

        public bool IsSelected {
            set {
                isSelected = value;
                NotifyPropertyChanged(nameof(IsSelected));
            }
            get => isSelected;
        }

        public FileSystemNodeModel(string path) {
            if (DriveInfo.GetDrives().Any(drive => drive.Name == path)) {
                Name = path;
            }
            else {
                Name = System.IO.Path.GetFileName(path);
            }
            NotifyPropertyChanged(nameof(Name));

            Path = path;
            NotifyPropertyChanged(nameof(Path));

            LoadThumb();
            NotifyPropertyChanged(nameof(Thumb));

            isExpanded = false;
            isSelected = false;
        }

        public IEnumerable<string> GetDirectories() {
            try {
                return Directory.GetDirectories(Path);
            }
            catch (Exception) {
                return Array.Empty<string>();
            }
        }

        public IEnumerable<string> GetFiles() {
            try {
                return Directory.GetFiles(Path);
            }
            catch (Exception) {
                return Array.Empty<string>();
            }
        }

        public bool CanDelete() => !IsDirectory;

        public void RunDelete() => FileSystem.DeleteFile(Path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);

        private void LoadThumb() {
            if (Path.EndsWith(@".png") || Path.EndsWith(@".jpg")) {
                using (var fs = File.OpenRead(Path)) {
                    Thumb = new BitmapImage();
                    Thumb.BeginInit();
                    Thumb.CacheOption |= BitmapCacheOption.OnLoad;
                    Thumb.StreamSource = fs;
                    Thumb.DecodePixelWidth = 200;
                    Thumb.EndInit();
                }
            }
            else {
                Thumb = new BitmapImage();
                Thumb.BeginInit();
                Thumb.CacheOption = BitmapCacheOption.OnLoad;
                if (Directory.Exists(Path)) {
                    Thumb.StreamSource = Application.GetResourceStream(new Uri(@"Resources/folder.png", UriKind.Relative)).Stream;
                }
                else {
                    Thumb.StreamSource = Application.GetResourceStream(new Uri(@"Resources/other.png", UriKind.Relative)).Stream;
                }
                Thumb.DecodePixelWidth = 200;
                Thumb.EndInit();
            }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged(string name = @"") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        #endregion INotifyPropertyChanged
    }
}