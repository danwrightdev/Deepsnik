using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.Models
{
    public class SalesOrderBaseModel
    {
        public int? Id { get; set; }
        public string AccountReference { get; set; }
        public DateTime SalesOrderDate { get; set; }
        public bool VatInclusive { get; set; }
    }

    public class SalesOrderModel : SalesOrderBaseModel
    {
        public SalesOrderModel()
        {
            SalesOrderAddress = new AddressModel();
            SalesOrderDeliveryAddress = new AddressModel();
            SalesOrderItems = new List<SalesOrderItemModel>();
        }
        
        public int SalesOrderAddressId { get; set; }
        public int SalesOrderDeliveryAddressId { get; set; }
        public AddressModel SalesOrderAddress { get; set; }
        public AddressModel SalesOrderDeliveryAddress { get; set; }
        public List<SalesOrderItemModel> SalesOrderItems { get; set; }
    }
}
