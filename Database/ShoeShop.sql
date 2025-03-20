Create database ShoesShop

Use ShoesShop
Set dateformat dmy;

Create table tb_BangGiaSP
(
	ID int primary key identity,
	MinGia money,
	MaxGia money,
);
go

Create table tb_BangMauSP
(
	ID int primary key identity,
	MauSac nvarchar(100),
);
go

Create table tb_BangKichThuoc
(
	ID int primary key identity,
	KichThuoc float,
);
go

Create table tb_BangGioiTinh
(
	ID int primary key identity,
	GioiTinh nvarchar(100),
);
go

Create table tb_BangGiaShip
(
	ID int primary key identity,
	MinThanhTien money,
	MaxThanhTien money,
	Phi money,
);
go

Create table tb_DanhMuc
(
	ID int primary key identity,
	TenDanhMuc nvarchar(100),
	SoLuongMatHang int default 0,
	AnhMoTa ntext,
);
go

Create table tb_MatHang
(
	ID int primary key identity,
	ID_DanhMuc int references tb_DanhMuc(ID),
	TenMatHang nvarchar(100),
	Gia money,
	SoLuongSanPham int default 0,
	DaBan int default 0,
	GiamGia int, --bao nhiêu %
	MoTa ntext,
	AnhMoTa ntext,
	Rating float,
	NgayThem datetime,
);
go


Create table tb_AnhMatHang
(	
	ID int primary key identity,
	ID_MatHang int references tb_MatHang(ID),
	DuongDanAnh ntext,
);
go


Create table tb_SizeMatHang
(	
	ID int primary key identity,
	ID_MatHang int references tb_MatHang(ID),
	ID_KichThuoc int references tb_BangKichThuoc(ID),
);
go



Create table tb_MauMatHang
(	
	ID int primary key identity,
	ID_MatHang int references tb_MatHang(ID),
	ID_MauSac int references tb_BangMauSP(ID),
);
go

Create table tb_GioiTinhMatHang
(	
	ID int primary key identity,
	ID_MatHang int references tb_MatHang(ID),
	ID_GioiTinh int references tb_BangGioiTinh(ID),
);
go

Create table tb_SanPham
(
	ID int primary key identity,
	ID_MatHang int references tb_MatHang(ID),
	ID_KichCo int references tb_SizeMatHang(ID),
	ID_MauSac int references tb_MauMatHang(ID),
	ID_GioiTinh int references tb_GioiTinhMatHang(ID),
	TrangThai bit, --1: đã bán, 0: chưa bán
);
go


Create table tb_NguoiDung
(
	ID int primary key identity,
	TenDangNhap varchar(200),
	MatKhau varchar(100),
	TenHienThi nvarchar(200),
	SDT varchar(12),
	Email varchar(100),
	GioiTinh int, --1: Nam, 0: Nữ, -1: Khác
	NgaySinh date,
	DiaChi ntext,
	AnhDaiDien ntext,
	TrangThai bit,
);
go

Create table tb_DonMuaHang
(
	ID int primary key identity,
	ID_NguoiDung int references tb_NguoiDung(ID),
	TrangThai int, --2: Đã nhận, 1: Đã duyệt, 0: đợi duyệt, -1: huỷ đơn
	HoTen nvarchar(200),
	SDT varchar(12),
	DiaChiGiaoHang ntext,
	NhaRieng bit,
	NgayDat datetime,
	NgayDuyet datetime,
	NgayDuKien datetime,
	TongTien money,
	TienShip money,
	TongCong money,
	HinhThucTT nvarchar(200),
);

Create table tb_MoTaDonHang
(
	ID int primary key identity,
	ID_DonMuaHang int references tb_DonMuaHang(ID),
	ID_MatHang int references tb_MatHang(ID),
	ID_KichCo int references tb_BangKichThuoc(ID),
	ID_MauSac int references tb_BangMauSP(ID),
	ID_GioiTinh int references tb_BangGioiTinh(ID),
	Gia money,
	GiamGia int,
	SoLuong int,
	ThanhTien money,
);
go

Create table tb_ChiTietDonMuaHang
(
	ID int primary key identity,
	ID_MoTaDonHang int references tb_MoTaDonHang(ID),
	ID_SanPham int references tb_SanPham(ID),
);
go

select * from tb_mathang

select * from tb_SizeMatHang

Create table tb_DanhGiaTraiNghiem
(
	ID int primary key identity,
	ID_NguoiDung int references tb_NguoiDung(ID),
	ID_MatHang int references tb_MatHang(ID),
	NoiDung ntext,
	Rating float, --bao nhiêu sao hoặc đánh giá trên thang điểm
	NgayDanhGia datetime,
);
go

create table tb_Admin(
  ID int identity (1,1) primary key,
  TenDangNhap nvarchar(100),
  MatKhau     nvarchar(100),
  HoTen       nvarchar(100),
)



create table tb_Banner(
  ID int identity (1,1) primary key,
  AnhBanner nvarchar(1000),
)

Insert into tb_Admin values ('a', 1, 'abcd')

----------------------------------------------
Insert into tb_BangGiaSP values (16, 200);
Insert into tb_BangGiaSP values (200, 500);
Insert into tb_BangGiaSP values (500, 1200);
Insert into tb_BangGiaSP values (1200, 5000);
Select * from tb_BangGiaSP
Delete tb_BangGiaSP;


----------------------------------------------
Insert into tb_BangMauSP values (N'Đen');
Insert into tb_BangMauSP values (N'Trắng');
Insert into tb_BangMauSP values (N'Hồng');
Insert into tb_BangMauSP values (N'Xanh dương')
Insert into tb_BangMauSP values (N'Đỏ');
Insert into tb_BangMauSP values (N'Vàng');


----------------------------------------------
Insert into tb_BangKichThuoc values (30);
Insert into tb_BangKichThuoc values (35);
Insert into tb_BangKichThuoc values (40);
Insert into tb_BangKichThuoc values (45);


----------------------------------------------
Insert into tb_BangGioiTinh values (N'Nam');
Insert into tb_BangGioiTinh values (N'Nữ');
Insert into tb_BangGioiTinh values (N'Unisex ');


----------------------------------------------
Insert into tb_DanhMuc values (N'Nike', 0, N'Nike.jpg');
Insert into tb_DanhMuc values (N'Adidas', 0, N'Adidas.jpg');
Insert into tb_DanhMuc values (N'Puma', 0, N'Puma.jpg');
Insert into tb_DanhMuc values (N'Reebok', 0, N'Reebok.jpg');
Insert into tb_DanhMuc values (N'Vans', 0, N'Vans.jpg');
Insert into tb_DanhMuc values (N'Converse', 0, N'Converse.jpg');
Insert into tb_DanhMuc values (N'New Balance', 0, N'New Balance.jpg');
Insert into tb_DanhMuc values (N'Under Armour', 0, N'Under Armour.jpg');
Insert into tb_DanhMuc values (N'Skechers', 0, N'Skechers.jpg');
Insert into tb_DanhMuc values (N'Asics', 0, N'Asics.jpg');
Insert into tb_DanhMuc values (N'Balenciaga', 0, N'Balenciaga.jpg');
Insert into tb_DanhMuc values (N'Prada', 0, N'Prada.jpg');
Insert into tb_DanhMuc values (N'Fila', 0, N'Fila.jpg');
Insert into tb_DanhMuc values (N'Salomon', 0, N'Salomon.jpg');
Insert into tb_DanhMuc values (N'Timberland', 0, N'Timberland.jpg');

Select * from tb_DanhMuc;


----------------------------------------------
Select * from tb_MatHang;
Insert into tb_MatHang values (1, N'Nike Air Max 90', 4249, 0, 0, 10, N'abcdefghdhsdfsifs', N'Nike Air Max 90.jpg', 3.5, GETDATE());
Insert into tb_MatHang values (1, N'Nike Air Max 95', 4699, 0, 0, 5, N'abcdefghdhsdfsifs', N'Nike Air Max 95.jpg', 4, GETDATE());
Insert into tb_MatHang values (1, N'Nike Air Max 270', 4699, 0, 0, 0, N'abcdefghdhsdfsifs', N'Nike Air Max 270.jpg', 5, GETDATE());

Insert into tb_MatHang values (1, N'Adidas NMD', 3970, 0, 0, 15, N'abcdefghdhsdfsifs', N'Adidas NMD.jpg', 4, GETDATE());
Insert into tb_MatHang values (1, N'Adidas Stan Smith', 2660, 0, 0, 30, N'abcdefghdhsdfsifs', N'Adidas Stan Smith.jpg', 5, GETDATE());
Insert into tb_MatHang values (1, N'Adidas Superstar', 2640, 0, 0, 10, N'abcdefghdhsdfsifs', N'Adidas Superstar.jpg', 2, GETDATE());
Insert into tb_MatHang values (1, N'Adidas Ultraboost', 2975, 0, 0, 3, N'abcdefghdhsdfsifs', N'Adidas Ultraboost.jpg', 4, GETDATE());
Insert into tb_MatHang values (1, N'Adidas Yeezy', 3725, 0, 0, 25, N'abcdefghdhsdfsifs', N'Adidas Yeezy.jpg', 5, GETDATE());


----------------------------------------------
Insert into tb_AnhMatHang values (4, N'Adidas Gazelle 1.jpg');
Insert into tb_AnhMatHang values (4, N'Adidas Gazelle 2.jpg');
Insert into tb_AnhMatHang values (4, N'Adidas Gazelle 3.jpg');
Insert into tb_AnhMatHang values (4, N'Adidas Gazelle 4.jpg');

Insert into tb_AnhMatHang values (5, N'Adidas NMD 1.jpg');
Insert into tb_AnhMatHang values (5, N'Adidas NMD 2.jpg');
Insert into tb_AnhMatHang values (5, N'Adidas NMD 3.jpg');
Insert into tb_AnhMatHang values (5, N'Adidas NMD 4.jpg');

Insert into tb_AnhMatHang values (6, N'Adidas Stan Smith 1.jpg');
Insert into tb_AnhMatHang values (6, N'Adidas Stan Smith 2.jpg');

Insert into tb_AnhMatHang values (7, N'Adidas Superstar 1.jpg');
Insert into tb_AnhMatHang values (7, N'Adidas Superstar 2.jpg');
Insert into tb_AnhMatHang values (7, N'Adidas Superstar 3.jpg');

Insert into tb_AnhMatHang values (8, N'Adidas Ultraboost 1.jpg');
Insert into tb_AnhMatHang values (8, N'Adidas Ultraboost 2.jpg');
Insert into tb_AnhMatHang values (8, N'Adidas Ultraboost 3.jpg');

Insert into tb_AnhMatHang values (9, N'Adidas Yeezy 1.jpg');
Insert into tb_AnhMatHang values (9, N'Adidas Yeezy 2.jpg');


----------------------------------------------
Insert into tb_SizeMatHang values (1, 1);
Insert into tb_SizeMatHang values (1, 2);
Insert into tb_SizeMatHang values (1, 3);
Insert into tb_SizeMatHang values (1, 4);

Insert into tb_SizeMatHang values (2, 1);
Insert into tb_SizeMatHang values (2, 2);
Insert into tb_SizeMatHang values (2, 3);
Insert into tb_SizeMatHang values (2, 4);

Insert into tb_SizeMatHang values (3, 1);
Insert into tb_SizeMatHang values (3, 2);
Insert into tb_SizeMatHang values (3, 3);
Insert into tb_SizeMatHang values (3, 4);

Insert into tb_SizeMatHang values (4, 1);
Insert into tb_SizeMatHang values (4, 2);
Insert into tb_SizeMatHang values (4, 3);
Insert into tb_SizeMatHang values (4, 4);

Insert into tb_SizeMatHang values (5, 1);
Insert into tb_SizeMatHang values (5, 2);
Insert into tb_SizeMatHang values (5, 3);
Insert into tb_SizeMatHang values (5, 4);

Insert into tb_SizeMatHang values (6, 1);
Insert into tb_SizeMatHang values (6, 2);
Insert into tb_SizeMatHang values (6, 3);
Insert into tb_SizeMatHang values (6, 4);

Insert into tb_SizeMatHang values (7, 1);
Insert into tb_SizeMatHang values (7, 2);
Insert into tb_SizeMatHang values (7, 3);
Insert into tb_SizeMatHang values (7, 4);

Insert into tb_SizeMatHang values (8, 1);
Insert into tb_SizeMatHang values (8, 2);
Insert into tb_SizeMatHang values (8, 4);

Insert into tb_SizeMatHang values (9, 3);

----------------------------------------------
Insert into tb_MauMatHang values (1, 1);
Insert into tb_MauMatHang values (1, 2);
Insert into tb_MauMatHang values (1, 3);

Insert into tb_MauMatHang values (2, 1);
Insert into tb_MauMatHang values (2, 2);
Insert into tb_MauMatHang values (2, 3);
Insert into tb_MauMatHang values (2, 4);

Insert into tb_MauMatHang values (3, 1);
Insert into tb_MauMatHang values (3, 2)
Insert into tb_MauMatHang values (3, 4);

Insert into tb_MauMatHang values (4, 1);
Insert into tb_MauMatHang values (4, 3);
Insert into tb_MauMatHang values (4, 4);

Insert into tb_MauMatHang values (5, 1);
Insert into tb_MauMatHang values (5, 4);

Insert into tb_MauMatHang values (6, 1);
Insert into tb_MauMatHang values (6, 2);
Insert into tb_MauMatHang values (6, 4);

Insert into tb_MauMatHang values (7, 1);
Insert into tb_MauMatHang values (7, 4);

Insert into tb_MauMatHang values (8, 4);

Insert into tb_MauMatHang values (9, 1);


----------------------------------------------
Insert into tb_GioiTinhMatHang values (1, 1);
Insert into tb_GioiTinhMatHang values (1, 2);
Insert into tb_GioiTinhMatHang values (1, 3);

Insert into tb_GioiTinhMatHang values (2, 1);
Insert into tb_GioiTinhMatHang values (2, 3);

Insert into tb_GioiTinhMatHang values (3, 2)
Insert into tb_GioiTinhMatHang values (3, 3);

Insert into tb_GioiTinhMatHang values (4, 1);
Insert into tb_GioiTinhMatHang values (4, 3);

Insert into tb_GioiTinhMatHang values (5, 3);

Insert into tb_GioiTinhMatHang values (6, 1);
Insert into tb_GioiTinhMatHang values (6, 3);

Insert into tb_GioiTinhMatHang values (7, 1);
Insert into tb_GioiTinhMatHang values (7, 2);

Insert into tb_GioiTinhMatHang values (8, 1);
Insert into tb_GioiTinhMatHang values (8, 2);
Insert into tb_GioiTinhMatHang values (8, 3);

Insert into tb_GioiTinhMatHang values (9, 1);

----------------------------------------------
Select * from tb_SanPham
select * from tb_SizeMatHang

Insert into tb_SanPham values (1, 1, 1, 1, 0)
Insert into tb_SanPham values (1, 1, 1, 1, 0)

----------------------------------------------
Insert into tb_NguoiDung values ('a', '1', N'HA Admin', 0967657011, N'abcd@gmail.com', 1, '20/08/2003', N'14/12 kp Bình Phước A, Bình Chuẩn, Thuận An, Bình Dương');
Select * from tb_NguoiDung

---------------------------------------------

----------------------------------------------
Select * from tb_DanhGiaTraiNghiem
delete tb_DanhGiaTraiNghiem
drop table tb_DanhGiaTraiNghiem

select * from tb_DonMuaHang
select * from tb_MoTaDonHang
select * from tb_ChiTietDonMuaHang

select CAST(NgayDuyet As date),SUM(TongCong) from tb_DonMuaHang group by NgayDuyet

delete tb_ChiTietDonMuaHang
delete tb_MoTaDonHang
delete tb_DonMuaHang

update tb_SanPham set TrangThai = 0

select * from tb_bangkichthuoc

delete tb_sanpham
delete tb_SizeMatHang
delete tb_MauMatHang
delete tb_GioiTinhMatHang
