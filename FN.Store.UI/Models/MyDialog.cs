using System;
using System.ComponentModel.DataAnnotations;

namespace MS.Web.UI.Models
{
    public class MyDialog
    {
        public enum DialogType : short
        {
            Info = 0,
            Success = 1,
            Warning = 2,
            Error = 3
        }

        /// <summary>
        /// Campos da MyDialog
        /// </summary>
        public string Title { get; set; }
        public string Content { get; set; }
        public DialogType @Type { get; set; }

        public override string ToString() => $"{{ \"title\": \"{Title}\", \"content\": \"{Content}\", \"type\": \"{@Type.ToString().ToLower()}\" }}";
    }
}
