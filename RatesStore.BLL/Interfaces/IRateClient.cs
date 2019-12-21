using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatesStore.BLL.Interfaces
{
    public interface IRateClient<T>
    {
        T GetAll();
        T GetAllForContcreteBase(string baseRate);
    }
}

