﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CuaHangBanGiay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class tb_DanhMuc
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tb_DanhMuc()
        {
            this.tb_MatHang = new HashSet<tb_MatHang>();
        }
    
        public int ID { get; set; }
        [DisplayName("Tên danh mục")]
        public string TenDanhMuc { get; set; }

        [DisplayName("Số lượng mặt hàng")]
        public Nullable<int> SoLuongMatHang { get; set; }

        [DisplayName("Ảnh mô tả")]
        public string AnhMoTa { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_MatHang> tb_MatHang { get; set; }
    }
}
