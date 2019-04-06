using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using ProductSearch.Models;
namespace ProductSearch.Controllers
{
    public class ProductController : Controller
    {
        XmlDocument doc = new XmlDocument();
        private readonly ApplicationDbContext _applicationDbContext;

        public ProductController()
        {
            _applicationDbContext = new ApplicationDbContext();
        }
        public ActionResult Index()
        {
            return View(_applicationDbContext.Products.ToList());
        }

        [HttpPost]
        public JsonResult AdvanceSearch(List<string> ironcontentmargins, string particlesize,
                                                string ironcontent, string reactivity, string particlesizemin,
                                                string particlesizemax)
        {
            var product = new Product();
            var products = new List<Product>();
            if (!String.IsNullOrWhiteSpace(particlesize))
                product.ParticleSize = Convert.ToDouble(particlesize);
            if (!String.IsNullOrWhiteSpace(ironcontent))
                product.IronContent = Convert.ToDouble(ironcontent);
            if (!String.IsNullOrWhiteSpace(reactivity))
                product.Reactivitiy = Convert.ToDouble(reactivity);
            if (!String.IsNullOrWhiteSpace(particlesizemin))
                product.ParticleSizeMin = Convert.ToDouble(particlesizemin);
            if (!String.IsNullOrWhiteSpace(particlesizemax))
                product.ParticleSizeMax = Convert.ToDouble(particlesizemax);
            var productsFilteredList = GetProductsFilteredList(product);
            if (ironcontentmargins == null)
                products = productsFilteredList.ToList();
            else
            {
                foreach (var ironcontents in ironcontentmargins)
                {
                    switch (ironcontents)
                    {
                        case "1-50":
                            products.AddRange(productsFilteredList.Where(x => x.IronContent >= 1 && x.IronContent <= 50).ToList());
                            break;
                        case "51-60":
                            products.AddRange(productsFilteredList.Where(x => x.IronContent >= 51 && x.IronContent <= 60).ToList());
                            break;
                        case "61-70":
                            products.AddRange(productsFilteredList.Where(x => x.IronContent >= 61 && x.IronContent <= 70).ToList());
                            break;
                        case "71-80":
                            products.AddRange(productsFilteredList.Where(x => x.IronContent >= 71 && x.IronContent <= 80).ToList());
                            break;
                        case "81-90":
                            products.AddRange(productsFilteredList.Where(x => x.IronContent >= 81 && x.IronContent <= 90).ToList());
                            break;
                        case "91-100":
                            products.AddRange(productsFilteredList.Where(x => x.IronContent >= 91 && x.IronContent <= 100).ToList());
                            break;
                        default:
                            break;
                    }
                }

            }
            return Json(products.OrderBy(o => o.RecordId));
        }
        [HttpPost]
        public JsonResult MixSearch(string ironcontent)
        {
            var productsFilteredList = _applicationDbContext.Products.ToList();
            List<ProductString> productsFilteredListString = new List<ProductString>();
            Product productModelNew = new Product();
            var productsFilteredListUpdated = productsFilteredList;
            int i = 1;
            foreach (var product in productsFilteredList.ToList())
            {
                var productIronContent = product.IronContent;
                var ironContentNew = Convert.ToInt16(ironcontent) * 2 - productIronContent;
                productModelNew = productsFilteredListUpdated.Where(x => x.IronContent == ironContentNew).FirstOrDefault();
                if (ironContentNew == productIronContent)
                {
                    ProductString productInString = new ProductString();
                    productInString.Id = i;
                    productInString.ParticleSize = product.ParticleSize.ToString();
                    productInString.IronContent = product.IronContent.ToString();
                    productInString.Reactivitiy = product.Reactivitiy.ToString();
                    productInString.Price = product.Price;
                    productInString.SurfaceArea = product.SurfaceArea.ToString();
                    productInString.Logevity = product.Logevity.ToString();
                    productInString.Media = product.Media;
                    productInString.Location = product.Location;
                    productsFilteredListString.Add(productInString);
                    i++;
                }
                else
                {
                    if (productModelNew != null)
                    {
                        ProductString productInString = new ProductString();
                        productInString.Id = i;
                        if (product.ParticleSize == productModelNew.ParticleSize)
                            productInString.ParticleSize = product.ParticleSize.ToString();
                        else
                            productInString.ParticleSize = product.ParticleSize + " - " + productModelNew.ParticleSize;
                        if (product.IronContent == productModelNew.IronContent)
                            productInString.IronContent = product.IronContent.ToString();
                        else
                            productInString.IronContent = product.IronContent + " - " + productModelNew.IronContent;
                        if (product.Reactivitiy == productModelNew.Reactivitiy)
                            productInString.Reactivitiy = product.Reactivitiy.ToString();
                        else
                            productInString.Reactivitiy = product.Reactivitiy + " - " + productModelNew.Reactivitiy;
                        productInString.Price = (product.Price + productModelNew.Price) / 2;
                        if (product.SurfaceArea == productModelNew.SurfaceArea)
                            productInString.SurfaceArea = product.SurfaceArea.ToString();
                        else
                            productInString.SurfaceArea = product.SurfaceArea + " - " + productModelNew.SurfaceArea;
                        if (product.Logevity == productModelNew.Logevity)
                            productInString.Logevity = product.Logevity.ToString();
                        else
                            productInString.Logevity = product.Logevity + " - " + productModelNew.Logevity;
                        if (product.Media.Equals(productModelNew.Media))
                            productInString.Media = product.Media;
                        else
                            productInString.Media = product.Media + " - " + productModelNew.Media;
                        if (product.Location.Equals(productModelNew.Location))
                            productInString.Location = product.Location;
                        else
                            productInString.Location = product.Location + " - " + productModelNew.Location;
                        productsFilteredListUpdated.Remove(product);
                        productsFilteredListUpdated.Remove(productModelNew);
                        productsFilteredListString.Add(productInString);
                        i++;
                    }
                }

            }
            return Json(productsFilteredListString.OrderBy(o => o.Id));
        }
        public List<Product> GetProductsFilteredList(Product model)
        {
            var productsFilteredList = new List<Product>();
            productsFilteredList = _applicationDbContext.Products.ToList();

            if (model.ParticleSize != 0 && model.Reactivitiy != 0 && model.IronContent != 0)
            {
                productsFilteredList = productsFilteredList.Where(x => (x.ParticleSize == model.ParticleSize) && (x.Reactivitiy == model.Reactivitiy) && (x.IronContent == model.IronContent)).ToList();
            }
            else if (model.ParticleSize != 0 && model.Reactivitiy != 0)
            {
                productsFilteredList = productsFilteredList.Where(x => (x.ParticleSize == model.ParticleSize) && (x.Reactivitiy == model.Reactivitiy)).ToList();
            }
            else if (model.ParticleSize != 0 && model.IronContent != 0)
            {
                productsFilteredList = productsFilteredList.Where(x => (x.ParticleSize == model.ParticleSize) && (x.IronContent == model.IronContent)).ToList();
            }
            else if (model.Reactivitiy != 0 && model.IronContent != 0)
            {
                productsFilteredList = productsFilteredList.Where(x => (x.Reactivitiy == model.Reactivitiy) && (x.IronContent == model.IronContent)).ToList();
            }
            else
            {
                if (model.ParticleSize != 0)
                {
                    productsFilteredList = productsFilteredList.Where(x => x.ParticleSize == model.ParticleSize).ToList();
                }
                else if (model.Reactivitiy != 0)
                {
                    productsFilteredList = productsFilteredList.Where(x => x.Reactivitiy == model.Reactivitiy).ToList();
                }
                else if (model.IronContent != 0)
                {
                    productsFilteredList = productsFilteredList.Where(x => x.IronContent == model.IronContent).ToList();
                }
                else if (model.ParticleSizeMin != 0 && model.ParticleSizeMax != 0)
                {
                    productsFilteredList = productsFilteredList.Where(x => x.ParticleSize >= model.ParticleSizeMin && x.ParticleSize <= model.ParticleSizeMax).ToList();
                }
            }
            return productsFilteredList;
        }

        public ActionResult ImportFromXML()
        {
            return View();
        }
        public ActionResult ProductsList()
        {
            return View(_applicationDbContext.Products.ToList());
        }
        public ActionResult EditProduct(int Id)
        {
            var productToEdit = _applicationDbContext.Products.Single(x => x.RecordId == Id);
            return View(productToEdit);
        }
        [HttpPost]
        public ActionResult SaveProduct(Product product)
        {
            var productToSave = _applicationDbContext.Products.Single(x => x.RecordId == product.RecordId);
            productToSave.ProductName = product.ProductName;
            productToSave.ParticleSize = product.ParticleSize;
            productToSave.IronContent = product.IronContent;
            productToSave.Reactivitiy = product.Reactivitiy;
            productToSave.Price = product.Price;
            productToSave.Logevity = product.Logevity;
            productToSave.Media = product.Media;
            productToSave.Location = product.Location;
            productToSave.Quantity = product.Quantity;
            _applicationDbContext.SaveChanges();
            return RedirectToAction("ProductsList");
        }
        public ActionResult AddNewProduct()
        {
            return View();
        }
        public ActionResult DeleteProduct(int Id)
        {
            var productToDelete = _applicationDbContext.Products.Single(x => x.RecordId == Id);
            _applicationDbContext.Products.Remove(productToDelete);
            _applicationDbContext.SaveChanges();
            return RedirectToAction("ProductsList");
        }
        [HttpPost]
        public ActionResult AddProductToDb(Product product)
        {
            product.AvailableDate = DateTime.Now;
            _applicationDbContext.Products.Add(product);
            _applicationDbContext.SaveChanges();
            return RedirectToAction("ProductsList");
        }

        [HttpPost]
        public ActionResult UploadData(HttpPostedFileBase files)
        {
            var appDataPath = Server.MapPath("~/App_Data/Uploads");
            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }
            string extension = Path.GetExtension(files.FileName);
            if (extension.Equals(".xml"))
            {
                if (files != null && files.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(files.FileName);
                    files.SaveAs(Path.Combine(Server.MapPath("~/App_Data/Uploads"), fileName));
                    AddDataToDB(files.FileName);
                }

            }
            return RedirectToAction("Index", "Home");

        }
        private void AddDataToDB(string fileName)
        {
            doc.Load(LoadFile(fileName));
            //Loop through the selected Nodes.
            foreach (XmlNode node in doc.SelectNodes("/NewDataSet/PRODUCTS"))
            {
                //Fetch the Node values and assign it to Model.
                _applicationDbContext.Products.Add(new Product
                {
                    RecordId = int.Parse(node["Record_No"].InnerText),
                    ParticleSize = Convert.ToDouble(node["Particle_Size"].InnerText),
                    IronContent = Convert.ToDouble(node["Iron_Content"].InnerText),
                    Reactivitiy = Convert.ToDouble(node["Reactivity"].InnerText),
                    Price = Convert.ToDouble(node["Price"].InnerText),
                    SurfaceArea = Convert.ToDouble(node["Surface_Area"].InnerText),
                    Logevity = Convert.ToDouble(node["Longevity"].InnerText),
                    Media = Convert.ToString(node["Media"].InnerText),
                    Location = Convert.ToString(node["Location"].InnerText),
                    AvailableDate = DateTime.Now
                });
            }
            _applicationDbContext.SaveChanges();
        }
        private string LoadFile(string fileName)
        {
            var path = Path.Combine(Server.MapPath("~/App_Data/Uploads"), fileName);
            return path;
        }
    }
}