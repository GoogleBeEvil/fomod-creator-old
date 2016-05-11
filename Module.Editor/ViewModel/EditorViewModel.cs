﻿using System.Collections.ObjectModel;
using System.Linq;
using AspectInjector.Broker;
using FomodInfrastructure.Aspect;
using FomodInfrastructure.MvvmLibrary.Commands;
using FomodModel.Base;
using FomodModel.Base.ModuleCofiguration;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Interactivity.InteractionRequest;
using System;

namespace Module.Editor.ViewModel
{
    [Aspect(typeof (AspectINotifyPropertyChanged))]
    public class EditorViewModel : BindableBase
    {
        #region Services

        private IRegionManager _regionManager;

        #endregion

        public EditorViewModel()
        {
            AddStep = new RelayCommand<ProjectRoot>(p =>
            {
                if (p.ModuleConfiguration.InstallSteps == null)
                    p.ModuleConfiguration.InstallSteps = new StepList();
                if (p.ModuleConfiguration.InstallSteps.InstallStep == null)
                    p.ModuleConfiguration.InstallSteps.InstallStep = new ObservableCollection<InstallStep>();
                p.ModuleConfiguration.InstallSteps.InstallStep.Add(new InstallStep {Name = "New Step"});
            });
            RemoveStep = new RelayCommand<object[]>(p =>
            {
                var parent = (ProjectRoot) p[0];
                var child = (InstallStep) p[1];
                parent.ModuleConfiguration.InstallSteps.InstallStep.Remove(child);
            });

            AddGroup = new RelayCommand<InstallStep>(p =>
            {
                if (p.OptionalFileGroups == null)
                    p.OptionalFileGroups = new GroupList();
                if (p.OptionalFileGroups.Group == null)
                    p.OptionalFileGroups.Group = new ObservableCollection<Group>();
                p.OptionalFileGroups.Group.Add(new Group {Name = "New Group"});
            });
            RemoveGroup = new RelayCommand<object[]>(p =>
            {
                var parent = (InstallStep) p[0];
                var child = (Group) p[1];
                parent.OptionalFileGroups.Group.Remove(child);
            });

            AddPlugin = new RelayCommand<Group>(p =>
            {
                if (p.Plugins == null)
                    p.Plugins = new PluginList();
                if (p.Plugins.Plugin == null)
                    p.Plugins.Plugin = new ObservableCollection<Plugin>();
                p.Plugins.Plugin.Add(new Plugin {Name = "New Plugin"});
            });
            RemovePlugin = new RelayCommand<object[]>(p =>
            {
                var parent = (Group) p[0];
                var child = (Plugin) p[1];
                parent.Plugins.Plugin.Remove(child);
            });


            DeleteDialogCommand = new RelayCommand<object[]>(obj =>
            {
                var item = obj[1];

                this.ConfirmationRequest.Raise( new Confirmation { Content = "Вы точно хотите удалить "+ item.GetType().Name + " узел?", Title = "Удалить узел?" }, c => {
                    if (c.Confirmed)
                    {
                        if (item is Plugin)
                            RemovePlugin.Execute(obj);
                        else if (item is Group)
                            RemoveGroup.Execute(obj);
                        else if (item is InstallStep)
                            RemoveStep.Execute(obj);
                        else
                            throw new NotImplementedException();
                    }
                });
            });
        }

       

        public string Header { get; private set; } = "Редактор";

        public void ConfigurateViewModel(IRegionManager regionManager, ProjectRoot projectRoot, string header = null)
        {
            _regionManager = regionManager;

            if (header != null)
                Header = header;
            else
            {
                var name = projectRoot.ModuleInformation.Name;
                Header = string.IsNullOrWhiteSpace(name) ? Header : name;
            }

            var pRoot = Data.FirstOrDefault(i => i.FolderPath == projectRoot.FolderPath);
            if (pRoot == null)
                Data.Add(projectRoot);
        }

        #region Properties

        private object _selectedNode;

        public object SelectedNode
        {
            get { return _selectedNode; }
            set
            {
                _selectedNode = value;
                if (value == null) return;

                var name = value.GetType().Name;

                var param = new NavigationParameters
                {
                    {name, value}
                };
                _regionManager.Regions["NodeRegion"].RequestNavigate(name + "View", param);
            }
        }

        public ObservableCollection<ProjectRoot> Data { get; set; } = new ObservableCollection<ProjectRoot>();

        #endregion

        #region Commands

        public RelayCommand<ProjectRoot> AddStep { get; }
        public RelayCommand<object[]> RemoveStep { get; }
        public RelayCommand<InstallStep> AddGroup { get; }
        public RelayCommand<object[]> RemoveGroup { get; }
        public RelayCommand<Group> AddPlugin { get; }
        public RelayCommand<object[]> RemovePlugin { get; }
        public RelayCommand<object[]> DeleteDialogCommand { get; private set; }

        #endregion



        public InteractionRequest<IConfirmation> ConfirmationRequest { get; private set; } = new InteractionRequest<IConfirmation>();
        
    }
}