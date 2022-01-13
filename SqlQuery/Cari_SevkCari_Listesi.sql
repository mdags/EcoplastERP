IF OBJECT_ID('tempdb..#liste') IS NOT NULL DROP TABLE #liste
create table #liste([Tipi] nvarchar(100), [Cari Kod] nvarchar(100), [Unvan] nvarchar(100), [Adres] nvarchar(max), [Ýl] nvarchar(100), [Ýlçe] nvarchar(100), [Vergi Dairesi] nvarchar(100), [Vergi No] nvarchar(100), [TC Kimlik No] nvarchar(100), [Tel] nvarchar(100), [Faks] nvarchar(100))
declare @oid uniqueidentifier
declare cr cursor
for select Oid from Contact where GCRecord is null and ContactType = '0902BF92-6D1A-4222-9E18-E352EFA62B07' 
open cr
fetch next from cr into @oid
while @@fetch_status = 0
begin
	insert into #liste([Tipi], [Cari Kod], [Unvan], [Adres], [Ýl], [Ýlçe], [Vergi Dairesi], [Vergi No], [TC Kimlik No], [Tel], [Faks])
	select 'Ana Cari', C.Code, C.Name, C.[Address], (select Name from City where GCRecord is null and Oid = C.City), (select Name from District where GCRecord is null and Oid = C.District), C.TaxOffice, C.TaxNumber, C.IdentityNumber, C.Phone, C.Fax from Contact C where C.Oid = @oid
	
	insert into #liste([Tipi], [Cari Kod], [Unvan], [Adres], [Ýl], [Ýlçe], [Vergi Dairesi], [Vergi No], [TC Kimlik No], [Tel], [Faks])
	select 'Sevk Carisi', C.Code, C.Name, C.[Address], (select Name from City where GCRecord is null and Oid = C.City), (select Name from District where GCRecord is null and Oid = C.District), C.TaxOffice, C.TaxNumber, C.IdentityNumber, C.Phone, C.Fax from Contact C where GCRecord is null and ContactType = '6014D933-DA5A-4E39-92B5-FF8E6A53258D' and C.MainContact = @oid
	
	fetch next from cr into @oid
end
close cr
deallocate cr

delete temp_cariliste
insert into temp_cariliste([Tipi], [Cari Kod], [Unvan], [Adres], [Ýl], [Ýlçe], [Vergi Dairesi], [Vergi No], [TC Kimlik No], [Tel], [Faks])
select * from #liste
select * from temp_cariliste