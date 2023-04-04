using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Areas.Product.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace App.Areas.Product.Service
{
    public class CartService
    {
        private readonly IHttpContextAccessor _context;
        private readonly HttpContext HttpContext;

        // lưu chuỗi key Cart
        public const string CARTKEY = "cart";

        public CartService(IHttpContextAccessor context)
        {
            _context = context;
            HttpContext = context.HttpContext;
        }

    // Lấy cart từ Session (danh sách CartItem)
    public List<CartItem> GetCartItems () {

        var session = HttpContext.Session;
        string jsoncart = session.GetString (CARTKEY);
        if (jsoncart != null) {
            return JsonConvert.DeserializeObject<List<CartItem>> (jsoncart);
        }
        return new List<CartItem> ();
    }

    // Xóa cart khỏi session
    public void ClearCart () {
        var session = HttpContext.Session;
        session.Remove (CARTKEY);
    }

    // Lưu Cart (Danh sách CartItem) vào session
    public void SaveCartSession (List<CartItem> ls) {
        var session = HttpContext.Session;
        string jsoncart = JsonConvert.SerializeObject (ls);
        session.SetString (CARTKEY, jsoncart);
    }       
    }
}