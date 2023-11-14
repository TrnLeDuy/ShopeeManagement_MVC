USE [master]
GO
IF EXISTS (SELECT * FROM SYS.DATABASES WHERE NAME = 'TMDTdb')
DROP DATABASE [TMDTdb]
GO

CREATE DATABASE [TMDTdb]
GO

USE [TMDTdb]
GO
/* ---------- NGƯỜI DÙNG ---------- */

CREATE TABLE KHACHHANG (
    id_kh VARCHAR(255) PRIMARY KEY,
    ho_ten NVARCHAR(255) NULL,
    sdt CHAR(50) NULL,
    email VARCHAR(255) NULL,
    ngay_sinh DATE NULL,
    dia_chi NVARCHAR(MAX) NULL,
    username VARCHAR(255) UNIQUE NULL,
    [password] VARCHAR(255) NULL,
    avatar VARCHAR(255) NULL,
    tinh_trang_kh INT DEFAULT 1,
    diem_tich_luy INT DEFAULT 0
);





CREATE TABLE NGUOIBANHANG (
	id_nbh VARCHAR(255),
	ten_cua_hang NVARCHAR(MAX),
	trang_thai_ch INT NULL,
	id_kh VARCHAR(255),
	PRIMARY KEY (id_nbh),
	FOREIGN KEY (id_kh) REFERENCES KHACHHANG(id_kh)
);
/* ---------- NGƯỜI DÙNG ---------- */ 
/*------------------------------------------------------------*/
/* ---------- BỘ PHẬN QUẢN LÝ ---------- */
CREATE TABLE CHUCVU (
	id_cv VARCHAR(3),
	ten_cv NVARCHAR(255) NULL,
	PRIMARY KEY (id_cv)
);

CREATE TABLE PHONGBAN (
	id_pb INT IDENTITY(1,1),
	ten_pb NVARCHAR(255) NULL,
	PRIMARY KEY (id_pb)
);

CREATE TABLE NHANVIEN (
	id_nv INT IDENTITY(1,1),
	ho_ten NVARCHAR(255) NULL,
	sdt CHAR(50) NULL,
	email VARCHAR(255) NULL,
	ngay_sinh DATE NULL,
	dia_chi NVARCHAR(MAX) NULL,
	username VARCHAR(255) UNIQUE NULL,
	password VARCHAR(255) NULL,
	avatar VARCHAR(255) NULL,
	tinh_trang_nv INT NULL,
	id_cv VARCHAR(3),
	id_pb INT,
	PRIMARY KEY (id_nv),
	FOREIGN KEY (id_cv) REFERENCES CHUCVU(id_cv),
	FOREIGN KEY (id_pb) REFERENCES PHONGBAN(id_pb)
);
/* ---------- BỘ PHẬN QUẢN LÝ ---------- */
/*------------------------------------------------------------*/
/* ---------- SẢN PHẨM ---------- */
CREATE TABLE SANPHAM (
	id_sp INT IDENTITY(1,1),
	ten_sp NVARCHAR(MAX) NULL,
	gia_sp NUMERIC(19, 5) NULL,
	PRIMARY KEY (id_sp)
);

CREATE TABLE KICHCO (
	id_size INT IDENTITY(1,1),
	ten_size NVARCHAR(32) NULL,
	height FLOAT NULL,
	width FLOAT NULL,
	PRIMARY KEY (id_size)
);

CREATE TABLE THUONGHIEU (
	id_brand INT IDENTITY(1,1),
	ten_brand NVARCHAR(255) NULL,
	PRIMARY KEY (id_brand)
);

CREATE TABLE XUATXU (
	id_member VARCHAR(2),
	ten_member NVARCHAR(64),
	PRIMARY KEY (id_member)
);

CREATE TABLE NGANHHANG (
    id_nganhhang INT IDENTITY(1,1),
    ten_nganhhang NVARCHAR(255) NULL,
	PRIMARY KEY (id_nganhhang)
);

CREATE TABLE NGANHHANGCON (
    id_nhcon INT IDENTITY(1,1),
    ten_nhcon NVARCHAR(255) NULL,
    id_nganhhang INT,
	PRIMARY KEY (id_nhcon),
    FOREIGN KEY (id_nganhhang) REFERENCES NGANHHANG(id_nganhhang)
);

CREATE TABLE NGANHHANGCAP3 (
    id_nhc3 INT IDENTITY(1,1),
    ten_nhc3 NVARCHAR(255),
    id_nhcon INT,
	PRIMARY KEY (id_nhc3),
    FOREIGN KEY (id_nhcon) REFERENCES NGANHHANGCON(id_nhcon)
);

CREATE TABLE BANGMAU (
	id_color INT IDENTITY(1,1),
	ten_mau NVARCHAR(64) NULL,
	PRIMARY KEY (id_color)
);

CREATE TABLE CHITIETSP (
	id_ctsp INT IDENTITY(1,1),
	id_sp INT,
	id_nbh VARCHAR(255),
	id_size INT,
	id_color INT,
	id_brand INT,
	id_member VARCHAR(2),
	id_nganhhang INT,
	id_nhcon INT,
	id_nhc3 INT,
	so_luong INT NULL,
	PRIMARY KEY (id_ctsp),
	FOREIGN KEY (id_sp) REFERENCES SANPHAM (id_sp),
	FOREIGN KEY (id_nbh) REFERENCES NGUOIBANHANG (id_nbh),
	FOREIGN KEY (id_size) REFERENCES KICHCO (id_size),
	FOREIGN KEY (id_color) REFERENCES BANGMAU (id_color),
	FOREIGN KEY (id_brand) REFERENCES THUONGHIEU (id_brand),
	FOREIGN KEY (id_member) REFERENCES XUATXU (id_member),
	FOREIGN KEY (id_nganhhang) REFERENCES NGANHHANG (id_nganhhang),
	FOREIGN KEY (id_nhcon) REFERENCES NGANHHANGCON (id_nhcon),
	FOREIGN KEY (id_nhc3) REFERENCES NGANHHANGCAP3 (id_nhc3)
);
/* ---------- SẢN PHẨM ---------- */
/*------------------------------------------------------------*/
/* ---------- VOUCHER ---------- */
CREATE TABLE KHUYENMAI (
	id_voucher INT IDENTITY(1,1),
	ty_le_giam FLOAT NULL,
	ngay_tao DATETIME NULL,
	ngay_bat_dau DATETIME NULL,
	ngay_ket_thuc DATETIME NULL,
	so_luong INT NULL,
	id_nbh VARCHAR(255),
	id_nv INT,
	PRIMARY KEY (id_voucher),
	FOREIGN KEY (id_nbh) REFERENCES NGUOIBANHANG(id_nbh),
	FOREIGN KEY (id_nv) REFERENCES NHANVIEN(id_nv),
);
/* ---------- VOUCHER ---------- */
/*------------------------------------------------------------*/
/* ---------- ĐƠN HÀNG ---------- */
CREATE TABLE PTTT (
	id_pttt INT IDENTITY(1,1),
	ten_pttt NVARCHAR(255),
	PRIMARY KEY (id_pttt)
);

CREATE TABLE DONHANG (
	id_don INT IDENTITY(1,1),
	ngay_dat DATETIME NULL,
	trang_thai_dh INT NULL,
	tt_thanh_toan INT NULL,
	tong_cong NUMERIC(19, 5) NULL,
	thanh_tien NUMERIC(19, 5) NULL,
	id_pttt INT,
	id_kh VARCHAR(255),
	id_nbh VARCHAR(255),
	id_voucher INT,
	PRIMARY KEY (id_don),
	FOREIGN KEY (id_pttt) REFERENCES PTTT(id_pttt),
	FOREIGN KEY (id_kh) REFERENCES KHACHHANG(id_kh),
	FOREIGN KEY (id_nbh) REFERENCES NGUOIBANHANG(id_nbh),
	FOREIGN KEY (id_voucher) REFERENCES KHUYENMAI(id_voucher)
);

CREATE TABLE CHITIETDONHANG (
	id_don INT,
	id_ctsp INT,
	id_nbh VARCHAR(255),
	so_luong INT NULL,
	gia_tien NUMERIC(19, 5),
	PRIMARY KEY (id_don, id_ctsp, id_nbh),
	FOREIGN KEY (id_ctsp) REFERENCES CHITIETSP (id_ctsp),
	FOREIGN KEY (id_don) REFERENCES DONHANG(id_don)
);
/* ---------- ĐƠN HÀNG ---------- */
/*------------------------------------------------------------*/
/* ---------- VẬN CHUYỂN ---------- */
CREATE TABLE CTYVANCHUYEN (
	id_cty INT IDENTITY(1,1),
	ten_cty NVARCHAR(255) NULL,
	PRIMARY KEY (id_cty)
);

CREATE TABLE SHIPPER (
    id_shipper INT IDENTITY(1,1),
    ho_ten NVARCHAR(255) NULL,
    sdt CHAR(50) NULL,
    email VARCHAR(255) NULL,
    dia_chi NVARCHAR(MAX) NULL,
    trang_thai_shipper INT NULL,
    id_cty INT,
    PRIMARY KEY (id_shipper),
    FOREIGN KEY (id_cty) REFERENCES CTYVANCHUYEN(id_cty)
);

CREATE TABLE VANCHUYEN (
	id_vc INT IDENTITY(1,1),
	phi_vc NUMERIC(19, 5) NULL,
	id_don INT,
    id_shipper INT,
    ngay_giao_hang DATETIME NULL,
    trang_thai_giao_hang INT NULL,
	PRIMARY KEY (id_vc),
	FOREIGN KEY (id_don) REFERENCES DONHANG(id_don),
	FOREIGN KEY (id_shipper) REFERENCES SHIPPER(id_shipper)
);
/* ---------- VẬN CHUYỂN ---------- */
/*------------------------------------------------------------*/
/* ---------- QUÀ TẶNG ---------- */
CREATE TABLE QUATANG(
	id_qua INT IDENTITY(1,1),
	ten_qua NVARCHAR(255) NULL,
	so_diem_can INT NULL,
	mo_ta_qua NVARCHAR(MAX) NULL,
	PRIMARY KEY (id_qua)
);

CREATE TABLE DONHANG_QUATANG (
    id_don INT,
    id_qua INT,
    so_diem_da_su_dung INT,
    PRIMARY KEY (id_don, id_qua),
    FOREIGN KEY (id_don) REFERENCES DONHANG(id_don),
    FOREIGN KEY (id_qua) REFERENCES QUATANG(id_qua)
);
/* ---------- QUÀ TẶNG ---------- */
/*------------------------------------------------------------*/
 /* ---------- TIN TỨC ---------- */
CREATE TABLE THELOAITIN (
    id_the_loai INT IDENTITY(1,1),
    ten_the_loai NVARCHAR(255) NULL,
    PRIMARY KEY (id_the_loai)
);

CREATE TABLE TINTUC (
    id_tin_tuc INT IDENTITY(1,1),
    tieu_de NVARCHAR(255) NULL,
    noi_dung NVARCHAR(MAX) NULL,
    ngay_dang DATETIME NULL,
    id_theloai INT,
    PRIMARY KEY (id_tin_tuc),
    FOREIGN KEY (id_theloai) REFERENCES THELOAITIN(id_the_loai)
);

CREATE TABLE CHITIETTIN (
    id_tin_tuc INT,
    id_nv INT,
    PRIMARY KEY (id_tin_tuc, id_nv),
    FOREIGN KEY (id_tin_tuc) REFERENCES TINTUC(id_tin_tuc),
    FOREIGN KEY (id_nv) REFERENCES NHANVIEN(id_nv)
);
/* ---------- TIN TỨC ---------- */
/*------------------------------------------------------------*/
/*------------ KHU VỰC ------------*/

CREATE TABLE  KHUVUC (
	id_kv CHAR(2) PRIMARY KEY,
	ten_kv NVARCHAR(64) NULL,
	ten_day_du NVARCHAR(128) NULL,
	quoc_gia CHAR(2) NULL
);

CREATE TABLE QUANHUYEN (
	id_quan CHAR(3) PRIMARY KEY,
	ten_quan NVARCHAR(128) NULL,
	id_kv CHAR(2),
	FOREIGN KEY(id_kv) REFERENCES KHUVUC(id_kv)
);

CREATE TABLE PHUONGXA (
	id_px CHAR(5) PRIMARY KEY,
	ten_px NVARCHAR(128) NULL,
	id_quan CHAR(3),
	FOREIGN KEY (id_quan) REFERENCES QUANHUYEN(id_quan)
);
/*------------ KHU VỰC ------------*/