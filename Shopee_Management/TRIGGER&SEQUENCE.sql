CREATE SEQUENCE KHACHHANG_SEQUENCE
    AS INT
    START WITH 1
    INCREMENT BY 1
    MINVALUE 1
    NO MAXVALUE
    CYCLE;

CREATE OR ALTER TRIGGER trgInsertKHACHHANG	
ON KHACHHANG
INSTEAD OF INSERT
AS
BEGIN
    DECLARE @NewID VARCHAR(255);
    SELECT @NewID = 'KH' + RIGHT('0000000000' + CAST((SELECT ISNULL(MAX(CAST(RIGHT(id_kh, 10) AS INT)), 0) + 1 FROM KHACHHANG) AS VARCHAR), 10);

    INSERT INTO KHACHHANG (id_kh, sdt, email, username, [password])
    SELECT 
        CASE WHEN i.id_kh IS NULL THEN @NewID ELSE i.id_kh END,
        i.sdt, i.email, i.username, i.[password]
    FROM inserted i;
END;

CREATE SEQUENCE NGUOIBANHANG_SEQUENCE
    AS INT
    START WITH 1
    INCREMENT BY 1
    MINVALUE 1
    NO MAXVALUE
    CYCLE;
	
CREATE OR ALTER TRIGGER trgInsertNGUOIBANHANG
ON NGUOIBANHANG
INSTEAD OF INSERT
AS
BEGIN
    DECLARE @NewID VARCHAR(255);
    SELECT @NewID = 'NBH' + RIGHT('0000000000' + CAST((SELECT ISNULL(MAX(CAST(RIGHT(id_nbh, 10) AS INT)), 0) + 1 FROM NGUOIBANHANG) AS VARCHAR), 10);

    INSERT INTO NGUOIBANHANG(id_nbh, ten_cua_hang, trang_thai_ch, id_kh, sdt_ch, dia_chi_ch, hinh_cua_hang, email_ch)
    SELECT 
        CASE WHEN i.id_nbh IS NULL THEN @NewID ELSE i.id_nbh END,
        i.ten_cua_hang, i.trang_thai_ch, i.id_kh, i.sdt_ch, i.dia_chi_ch, i.hinh_cua_hang, i.email_ch
    FROM inserted i;
END;

/*Reset all the id  with identity 1,1*/
DECLARE @SqlCommand NVARCHAR(MAX) = N'';

SELECT @SqlCommand += N'DBCC CHECKIDENT(''' + TABLE_SCHEMA + '.' + TABLE_NAME + ''', RESEED, 0);' + CHAR(13)
FROM INFORMATION_SCHEMA.TABLES
WHERE OBJECTPROPERTY(OBJECT_ID(TABLE_NAME), 'TableHasIdentity') = 1;

EXEC sp_executesql @SqlCommand;