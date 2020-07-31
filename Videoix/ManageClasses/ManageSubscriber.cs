using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Videoix.ManageClasses
{
    public class ManageSubscriber
    {
        public event EventHandler OnExecute;
        private ManageSubscriber()
        {
            OnExecute = new EventHandler(ManageSubscriber_OnExecute);
        }
        private void ManageSubscriber_OnExecute(object sender, EventArgs e)
        {

        }
        public static ManageSubscriber Create()
        {
            return new ManageSubscriber();     
        }
    }
}
