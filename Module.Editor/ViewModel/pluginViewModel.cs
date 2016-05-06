﻿using System;
using System.Windows;
using System.Xml;
using AspectInjector.Broker;
using FomodInfrastructure.Aspect;
using FomodInfrastructure.MvvmLibrary.Commands;
using Microsoft.Practices.ServiceLocation;
using Module.Editor.Resource;
using Prism.Events;

namespace Module.Editor.ViewModel
{
    /// <summary>
    /// Base plugin info
    /// </summary>
    [Aspect(typeof(AspectINotifyPropertyChanged))]
    public partial class PluginViewModel
    {
        #region Services

        private IServiceLocator _serviceLocator;
        private IEventAggregator _eventAggregator;

        #endregion
        
        #region Commands

        public RelayCommand AddImage { get; private set; } 
        public RelayCommand RemoveImage { get; private set; } 

        #endregion

        public PluginViewModel(IServiceLocator serviceLocator, IEventAggregator eventAggregator)
        {
            CurentParamName = Names.PluginName;
            _serviceLocator = serviceLocator;
            _eventAggregator = eventAggregator;
            AddImage = new RelayCommand(() => { });  //TODO сделать проверку передаваемого параметра _nodePluginHelper.AddImage(p.ToString());
            RemoveImage = new RelayCommand(() => { }); //TODO сделать проверку передаваемого параметра или диалог выбора файлов (мнодественный) а также проверку форматов картинок (авось что то не то передадут) _nodePluginHelper.RemoveImage(p as XmlNode);
            FilesCtor();
            FlagsCtor();
        }
    }

    /// <summary>
    /// Files & Folders
    /// </summary>
    public partial class PluginViewModel
    {
        #region Commands

        public RelayCommand AddFileFolderGroup { get; private set; } 
        public RelayCommand RemoveFileFolderGroup { get; private set; }
        public RelayCommand RemoveFile { get; private set; }
        public RelayCommand AddFile { get; private set; }
        public RelayCommand AddFolder { get; private set; } 

        #endregion

        public bool IsFilesFoldersFlags { get; set; }

        private void FilesCtor()
        {
            AddFileFolderGroup = new RelayCommand(() => MessageBox.Show("Wow! Added files group"));
            RemoveFileFolderGroup = new RelayCommand(() => MessageBox.Show("Wow! Remove files group"));
            RemoveFile = new RelayCommand(() => MessageBox.Show("Wow! Remove FILE/FOLDER"));
            AddFile = new RelayCommand(() => MessageBox.Show("Wow! Added file"));
            AddFolder = new RelayCommand(() => MessageBox.Show("Wow! Added folder"));

            ThenSetXmlNode(xmlNode =>
            {
                if (chkFrament_var1(xmlNode))
                    IsFilesFoldersFlags = true;
                else if (chkFrament_var2(xmlNode))
                    IsFilesFoldersFlags = false;
                else
                    throw new NotImplementedException();
            });
        }

        // ReSharper disable PossibleNullReferenceException
        private static bool chkFrament_var2(XmlNode xdoc)
        {
            var cFlags = xdoc.SelectNodes("conditionFlags");
            var files = xdoc.SelectNodes("files");
            
            return cFlags.Count == 1 && (files.Count <= 0 || cFlags[0].NextSibling.Name == "files");
        }
        private static bool chkFrament_var1(XmlNode pluginNode)
        {
            var cFlags = pluginNode.SelectNodes("conditionFlags");
            var files = pluginNode.SelectNodes("files");

            return files.Count == 1 && (cFlags.Count <= 0 || files[0].NextSibling.Name == "conditionFlags");
        }
        // ReSharper restore PossibleNullReferenceException

    }

    /// <summary>
    /// Flags
    /// </summary>
    public partial class PluginViewModel : BaseViewModel
    {
        #region Commands

        public RelayCommand AddFlagsGroup { get; private set; }
        public RelayCommand RemoveFlagsGroup { get; private set; }
        public RelayCommand RemoveFlag { get; private set; }
        public RelayCommand AddFlag { get; private set; }

        #endregion

        private void FlagsCtor()
        {
            AddFlagsGroup = new RelayCommand(() => MessageBox.Show("Wow! Added flags group"));
            RemoveFlagsGroup = new RelayCommand(() => MessageBox.Show("Wow! Remove flags group"));
            RemoveFlag = new RelayCommand(() => MessageBox.Show("Wow! Remove FLAG"));
            AddFlag = new RelayCommand(() => MessageBox.Show("Wow! added FLAG"));
        }
    }
}