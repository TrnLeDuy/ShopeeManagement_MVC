USE [master]
GO
/*Delete old database with the same name*/
IF EXISTS (SELECT * FROM SYS.DATABASES WHERE NAME ='ecommerceDB_tt')
DROP DATABASE [ecommerceDB_tt]
GO
/*Create Database*/
CREATE DATABASE [ecommerceDB_tt]
GO
USE [ecommerceDB_tt]
GO
/*Create Table*/

/*TẠO BẢNG VỀ SẢN PHẨM*/
CREATE TABLE [categories] /*Loại sản phẩm*/
(
	categories_id int IDENTITY (1,1) PRIMARY KEY,
	categories_name nvarchar(128) NOT NULL
);
GO
CREATE TABLE [brand] /*Thương hiệu | Nhà cung cấp*/
(
	brand_id char(10) NOT NULL PRIMARY KEY,
	brand_name nvarchar(128) NULL
)
GO
CREATE TABLE [size] /*Kích cỡ sản phẩm*/
(
	size_id int IDENTITY(1,1) PRIMARY KEY,
	size_name nvarchar(128) NULL,
	size_sex bit NULL
)
GO
CREATE TABLE [color] /*Màu sắc sản phẩm*/
(
	color_id int IDENTITY (1,1) PRIMARY KEY,
	color_name nvarchar(128) NULL
)
GO
CREATE TABLE [product]  /*Sản Phẩm*/
(
	prod_id int IDENTITY (1,1) PRIMARY KEY,
	img_name varchar(100) NULL,
	prod_name nvarchar (500) NOT NULL,
	prod_info nvarchar (max) NULL,
	price numeric (19,5) NULL,
	[status] tinyint NOT NULL,
	material nvarchar (100) NULL,
	cus_target nvarchar (100) NULL, /*Đối tượng khách hàng*/

	categories_id int FOREIGN KEY REFERENCES categories(categories_id),
	brand_id char(10) FOREIGN KEY REFERENCES brand(brand_id)
)
GO
CREATE TABLE [prodQuantity] /*TỒN KHO*/
(
	prod_id int,
	size_id int,
	color_id int,
	quantity int DEFAULT 0,
	prodStatus bit DEFAULT 0, 

	PRIMARY KEY(prod_id, size_id, color_id),

	FOREIGN KEY (prod_id) REFERENCES product(prod_id),
	FOREIGN KEY (size_id) REFERENCES size(size_id),
	FOREIGN KEY (color_id) REFERENCES color(color_id)
)
GO

/*TẠO BẢNG VỀ NHÂN SỰ*/
CREATE TABLE [employee_position] /*Chức vụ*/
(
	job_id int IDENTITY (1,1) PRIMARY KEY,
	job_name nvarchar(128) NULL,
)
GO
CREATE TABLE [department] /*Phòng ban*/
(
	department_id int IDENTITY(1,1) PRIMARY KEY,
	department_name nvarchar(128) NULL
)
GO
CREATE TABLE [employee] 
(
	emp_id int IDENTITY (1,1) PRIMARY KEY,
	emp_name nvarchar(128) NOT NULL,
	emp_birth date NULL,
	emp_mail varchar(50) NULL,
	emp_phone char(12) NULL,
	emp_addr nvarchar(128) NULL,
	emp_gender bit NULL,
	emp_img varchar(500) NULL,
	job_id int NULL, /*Chức vụ*/
	department_id int NULL, /*Phòng ban*/
	[account] char(32) NOT NULL UNIQUE,
	[password] char(16) NOT NULL DEFAULT 999999,

	FOREIGN KEY (job_id) REFERENCES employee_position(job_id),
	FOREIGN KEY (department_id) REFERENCES department(department_id)
)
GO

/*TẠO BẢNG KHÁCH HÀNG*/
CREATE TABLE [customer_categories] /*Loại khách hàng*/
(
	custype_id int IDENTITY (1,1) PRIMARY KEY,
	custype_name nvarchar(128) NULL,
)
GO
CREATE TABLE [customer]
(
	customer_id char(10) NOT NULL PRIMARY KEY,
	customer_name nvarchar(128) NOT NULL,
	customer_birth date NULL,
	customer_age AS DATEDIFF(YEAR, customer_birth, getdate()),
	customer_mail varchar(50) NULL,
	customer_phone char(12) NULL,
	customer_addr nvarchar(128) NULL,
	customer_gender bit NULL,
	customer_img varchar(500) NULL,
	[account] char(32) NOT NULL UNIQUE,
	[password] char(16) NOT NULL,
	custype_id int NULL,

	FOREIGN KEY (custype_id) REFERENCES customer_categories(custype_id)
)
GO

/*HÓA ĐƠN MUA HÀNG*/
CREATE TABLE [bill]
(
	bill_id int IDENTITY (1,1) PRIMARY KEY,
	dateStart date NULL,
	payment nvarchar(128) NULL,
	bstatus nvarchar(128) NULL,
	totalPrice numeric(19,5) NULL,
	customer_id char(10) NOT NULL,
	emp_id int NOT NULL,

	FOREIGN KEY (customer_id) REFERENCES customer(customer_id),
	FOREIGN KEY (emp_id) REFERENCES employee(emp_id)
)
GO
CREATE TABLE [bill_detail]
(
	bill_id int,
	prod_id int,
	size_id int,
	color_id int,
	quantity int,
	totalpriceEach numeric (19,5),

	PRIMARY KEY(bill_id, prod_id, size_id, color_id),

	FOREIGN KEY (bill_id) REFERENCES bill(bill_id),
	FOREIGN KEY (prod_id, size_id, color_id) REFERENCES prodQuantity(prod_id, size_id, color_id)
)
GO