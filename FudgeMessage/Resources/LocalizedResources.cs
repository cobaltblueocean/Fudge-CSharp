// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by OpenGamma Inc.
//
// Copyright (C) 2012 - present by OpenGamma Inc. and the OpenGamma group of companies
//
// Please see distribution for license.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
//     
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FudgeMessage.Properties;

namespace FudgeMessage.Resources
{
    /// <summary>
    /// LocalizedResources Description
    /// </summary>
    public class LocalizedResources : INotifyPropertyChanged
    {
        #region Singleton Class Implementation
        private readonly Resource resources = new FudgeMessage.Properties.Resource();

        private static LocalizedResources instance;

        /// <summary>
        /// Constructor implement as Singleton pattern
        /// </summary>
        private LocalizedResources()
        {
        }

        /// <summary>
        /// Return singleton instance
        /// </summary>
        /// <returns>Return current instance</returns>
        public static LocalizedResources Instance()
        {
            if (instance == null)
                instance = new LocalizedResources();

            return instance;
        }

        /// <summary>
        /// Hangling culture changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Change resource culture change
        /// </summary>
        /// <param name="name"></param>
        public void ChangeCulture(string name)
        {
            Resource.Culture = CultureInfo.GetCultureInfo(name);
            RaisePropertyChanged("Resources");
        }

        /// <summary>
        /// Get resource
        /// </summary>
        internal Resource Resource
        {
            get { return resources; }
        }

        #endregion

        public String MismatchWithPropertyFound
        {
            get { return Resource.MismatchWithPropertyFound; }
        }


    }
}
