using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;

namespace WPFUI
{
    public class ProductHttpClientWrapper : IProductHttpClientWrapper
    {
        static HttpClient clientAPI = null;
        private ICategoryAction category;
        private IProductAction product;

        public ProductHttpClientWrapper()
        {
            if (clientAPI == null)
            {
                clientAPI = new HttpClient();
                IConfigurationRoot configurationRoot = new ConfigurationBuilder().SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
                string apiURL = configurationRoot["APISettings:URL"];
                clientAPI.BaseAddress = new Uri(apiURL);
                clientAPI.DefaultRequestHeaders.Accept.Clear();
                clientAPI.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
        }

        public ICategoryAction Category
        {
            get
            {
                if (category == null)
                {
                    category = new CategoryAction(clientAPI);
                }

                return category;
            }
        }

        public IProductAction Product
        {
            get
            {
                if (product == null)
                {
                    product = new ProductAction(clientAPI);
                }

                return product;
            }
        }

    }
}
