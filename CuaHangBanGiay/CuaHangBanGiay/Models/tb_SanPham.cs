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

    public partial class tb_SanPham
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tb_SanPham()
        {
            this.tb_ChiTietDonMuaHang = new HashSet<tb_ChiTietDonMuaHang>();
        }
    
        public int ID { get; set; }
        [DisplayName("ID mặt hàng")]
        public Nullable<int> ID_MatHang { get; set; }

        [DisplayName("ID kích cỡ")]
        public Nullable<int> ID_KichCo { get; set; }

        [DisplayName("ID màu sắc")]
        public Nullable<int> ID_MauSac { get; set; }

        [DisplayName("ID giới tính")]
        public Nullable<int> ID_GioiTinh { get; set; }

        [DisplayName("Trạng thái")]
        public Nullable<bool> TrangThai { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_ChiTietDonMuaHang> tb_ChiTietDonMuaHang { get; set; }
        public virtual tb_GioiTinhMatHang tb_GioiTinhMatHang { get; set; }
        public virtual tb_MatHang tb_MatHang { get; set; }
        public virtual tb_MauMatHang tb_MauMatHang { get; set; }
        public virtual tb_SizeMatHang tb_SizeMatHang { get; set; }
    }
}
