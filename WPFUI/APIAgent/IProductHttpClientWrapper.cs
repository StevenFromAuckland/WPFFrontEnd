using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WPFUI
{
    public interface IProductHttpClientWrapper
    {
        ICategoryAction Category { get; }
        IProductAction Product { get; }
    }
}
