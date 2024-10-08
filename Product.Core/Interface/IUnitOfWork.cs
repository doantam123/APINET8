﻿namespace Product.Core.Interface
{
    public interface IUnitOfWork
    {
        public ICategoryRepository CategoryRepository { get; }
        public IProductRepository ProductRepository { get; }
        public IBasketRepository BasketRepository { get; }
    }
}
