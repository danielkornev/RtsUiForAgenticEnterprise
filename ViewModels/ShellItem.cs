using MutliModalLiveCopilotWpfApp.Common;
using MutliModalLiveCopilotWpfApp.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace MutliModalLiveCopilotWpfApp.ViewModels
{
    /// <summary>
    /// This is a data representation of an item, not a visual one
    /// </summary>
    public class ShellItem : Drawable, ICloneable, IGroupable
    {
        private Group _group;

        public Group Group { get => _group; set => IsDraggable = value != null; }
       
        public object Clone() => JsonSerializer.Deserialize<ShellItem>(JsonSerializer.Serialize(this));

        private string _imageSource;

        public string Thumbnail
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
        }

        private string _title;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _description;

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private string _rawTextContent;

        public string RawTextContent
        {
            get => _rawTextContent;
            set => SetProperty(ref _rawTextContent, value);
        }

        private FlowDocument _flowDocumentContent;

        public FlowDocument FlowDocumentContent
        {
            get => _flowDocumentContent;
            set => SetProperty(ref _flowDocumentContent, value);
        }
    } // class
} // namespace
