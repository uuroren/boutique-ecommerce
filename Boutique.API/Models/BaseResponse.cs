namespace Boutique.API.Models {
    public class BaseResponse {
        public bool Success { get; set; }
        public string Message { get; set; }
        public dynamic Result { get; set; }
    }
}
