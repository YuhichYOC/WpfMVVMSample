/*
 * ContextMenuDeleteCommand.cs
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
using System.Windows.Input;

namespace WpfMVVMSample.ICommandImpl {

    internal class ContextMenuDeleteCommand : ICommand {
        private readonly Func<bool> canExecute;

        private readonly Action action;

        public ContextMenuDeleteCommand(Func<bool> canExecute, Action action) {
            this.canExecute = canExecute;
            this.action = action;
        }

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        #region ICommand

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? param) => canExecute();

        public void Execute(object? param) => action();

        #endregion ICommand
    }
}