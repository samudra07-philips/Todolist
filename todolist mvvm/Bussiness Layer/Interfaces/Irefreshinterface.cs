using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace todolist_mvvm.viewmodel
{
    public interface IRefreshablePage
    {
        void RefreshContent();
    }
}
