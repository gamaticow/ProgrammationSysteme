using System;
using System.Collections.Generic;
using System.Text;
using EasySave.Controller;

namespace EasySave.View
{
    abstract class View
    {

        protected Controller.Controller controller { get; private set; }

        public View(Controller.Controller controller)
        {
            this.controller = controller;
        }

        public void RenderError(string error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(controller.Translate(error));
            Console.ResetColor();
        }

        protected void RenderChoseAction()
        {
            Console.Write(controller.Translate("chose_action"));
        }

    }
}
