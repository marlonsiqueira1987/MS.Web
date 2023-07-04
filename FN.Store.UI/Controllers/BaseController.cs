using MS.Web.UI.Models;
using System;
using System.Web.Mvc;

namespace MS.Web.UI.Controllers
{   
    /// <summary>
    /// Classe criada para trabalhar com as mensagens entre Controllers e Views
    /// </summary>
    public abstract class BaseController : Controller
    {
        public const string SystemMessage = "MY_DIALOG";

        protected void ShowMessage(string htmlContent, string htmlTitle = "Mensagem do Sistema", MyDialog.DialogType type = MyDialog.DialogType.Info)
        {
            this.ShowMessage(new MyDialog { Title = htmlTitle, Content = htmlContent, @Type = type });
        }

        public void ShowMessage(MyDialog dialog)
        {
            this.TempData[SystemMessage] = dialog.ToString();
        }
    }
}