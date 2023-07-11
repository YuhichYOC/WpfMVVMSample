/*
 * MainWindowModel.cs
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

using System.ComponentModel;

namespace WpfMVVMSample.Model {

    internal class MainWindowModel : INotifyPropertyChanged {
        private int fontSize;

        private int thumbHeight;

        private int thumbWidth;

        public int FontSize {
            set {
                fontSize = value;
                NotifyPropertyChanged(nameof(FontSize));
            }
            get => fontSize;
        }

        public int ThumbHeight {
            set {
                thumbHeight = value;
                NotifyPropertyChanged(nameof(ThumbHeight));
                NotifyPropertyChanged(nameof(StackPanelItemHeight));
            }
            get => thumbHeight;
        }

        public int StackPanelItemHeight => thumbHeight + 40;

        public int ThumbWidth {
            set {
                thumbWidth = value;
                NotifyPropertyChanged(nameof(ThumbWidth));
                NotifyPropertyChanged(nameof(StackPanelItemWidth));
            }
            get => thumbWidth;
        }

        public int StackPanelItemWidth => thumbWidth + 60;

        public MainWindowModel() {
            fontSize = 11;
            thumbHeight = 100;
            thumbWidth = 100;
            NotifyPropertyChanged(nameof(FontSize));
            NotifyPropertyChanged(nameof(ThumbHeight));
            NotifyPropertyChanged(nameof(StackPanelItemHeight));
            NotifyPropertyChanged(nameof(ThumbWidth));
            NotifyPropertyChanged(nameof(StackPanelItemWidth));
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged(string name = @"") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        #endregion INotifyPropertyChanged
    }
}