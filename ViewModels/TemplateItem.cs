using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MutliModalLiveCopilotWpfApp.ViewModels
{
    class TemplateItem : ShellItem
    {
        private string _templateType;

        public string TemplateType
        {
            get => _templateType;
            set => SetProperty(ref _templateType, value);
        }
    } // class
} // namespace
