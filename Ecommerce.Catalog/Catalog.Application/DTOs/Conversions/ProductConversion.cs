using Catalog.Domain.Entities;

namespace Catalog.Application.DTOs.Conversions
{
    public static class ProductConversion
    {
        public static Product ToEntity(ProductDto product) => new()
        {
            ProductId = product.ProductId,
            Sku = product.Sku,
            ProductDescription = product.ProductDescription,
            ProductCategory = product.ProductCategory,
            Quantity = product.Quantity,
            Price = product.Price
        };

        public static (ProductDto?, IEnumerable<ProductDto>?) FromEntity(Product product, IEnumerable<Product>? products)
        {
            if (product != null && products == null)
            {
                ProductDto singleProduct = new ProductDto()
                {
                    ProductId = product.ProductId,
                    Sku = product.Sku,
                    ProductDescription = product.ProductDescription,
                    ProductCategory = product.ProductCategory,
                    Quantity = product.Quantity,
                    Price = product.Price
                };
                return (singleProduct, null);
            }
            if(product == null && products != null)
            {
                List<ProductDto> _products = products.Select(p => new ProductDto()
                {
                    ProductId = product.ProductId,
                    Sku = product.Sku,
                    ProductDescription = product.ProductDescription,
                    ProductCategory = product.ProductCategory,
                    Quantity = product.Quantity,
                    Price = product.Price
                }
                ).ToList();
                return (null, _products);
            }
            return (null,null);
        }
    }
}
