using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core;
using Talabat.Core.Entites;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;
using Talabat.Repository;
using Talabat.Repository.Data;
using TalabatAPIs.DTOS;
using TalabatAPIs.Errors;
using TalabatAPIs.Helpers;

namespace TalabatAPIs.Controllers
{
    public class ProductsController : ApiBaseController
    {

        //private readonly IGenericRepository<Product> _productrepo;
        //private readonly IGenericRepository<ProductTypes> _typesRepo;
        //private readonly IGenericRepository<ProductBrand> _brandsRepo;
        private readonly IUnitOFWork _unitOFWork;
        private readonly IMapper _mapper;

        public ProductsController(IUnitOFWork unitOFWork, IMapper mapper)
        {
            //_productrepo = Productrepo;
            //_typesRepo = TypesRepo;
            //_brandsRepo = BrandsRepo;
            _unitOFWork = unitOFWork;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        //public async Task<ActionResult<IReadOnlyList<ProductToReturnDTO>>> GetProducts([FromQuery] ProductSpecParams Params)
        public async Task<ActionResult<Pagination<ProductToReturnDTO>>> GetProducts([FromQuery] ProductSpecParams Params)
        {
            var spec = new ProductTypesBrandSpecification(Params);
            //var Products = await _productrepo.GetAllAsyncWithSpec(spec);
            var Products = await _unitOFWork.Repository<Product>().GetAllAsyncWithSpec(spec);
            var MappedProducts = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDTO>>(Products);
            var CountSpec = new ProductWithFiterationForCountAsync(Params);
            //var Count = await _productrepo.GetCountWithSpecAsync(CountSpec);
            var Count = await _unitOFWork.Repository<Product>().GetCountWithSpecAsync(CountSpec);
            return Ok(new Pagination<ProductToReturnDTO>(Params.PageIndex, Params.PageSize , MappedProducts , Count));
        }


        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductToReturnDTO), 200)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]

        //public async Task<ActionResult<Product>> GetProduct(int id)
        public async Task<ActionResult<ProductToReturnDTO>> GetProduct(int id)
        {
            //var Product = await _productrepo.GetByIdAsync(id);
            var spec = new ProductTypesBrandSpecification(id);
            //var Product = await _productrepo.GetByIdAsyncWithSpec(spec);
            var Product = await _unitOFWork.Repository<Product>().GetByIdAsyncWithSpec(spec);
            if (Product is null) return NotFound(new ApiResponse(404));
            var MappedProduct = _mapper.Map<Product, ProductToReturnDTO>(Product);
            return Ok(MappedProduct);
        }

        [HttpGet("Types")]
        //public async Task<ActionResult<IEnumerable<ProductTypes>>> GetTypes()
        public async Task<ActionResult<IReadOnlyList<ProductTypes>>> GetTypes()
        {
            //var Types = await _typesRepo.GetAllAsync();
            var Types = await _unitOFWork.Repository<ProductTypes>().GetAllAsync();
            return Ok(Types);
        }

        [HttpGet("Brands")]
        //public async Task<ActionResult<IEnumerable<ProductBrand>>> GetBrands()
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            //var Brands = await _brandsRepo.GetAllAsync();
            var Brands = await _unitOFWork.Repository<ProductBrand>().GetAllAsync();
            return Ok(Brands);
        }

        #region Deffrince between The DataTypes 
        // IEnumerable         => Retrive Data Only                  => When Iterate on List
        // IQueryable          => Retrive Data With Filtration       => With Database
        // IReadOnlyCollection => Retrive Data Only                  => With Count
        // ICollection         => Retrive Data And Do Crud Operation => When you need to do modification
        #endregion


    }
}
