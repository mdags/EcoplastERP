select T.PlateNumber as [Plaka], (select top 1 DorsePlate from Expedition where GCRecord is null and Truck = T.Oid) as [Dorse Plaka], (case when T.TruckType = 1 then 'Kamyonet' when T.TruckType = 2 then 'On Teker' when T.TruckType = 3 then 'K�rk Ayak' when T.TruckType = 4 then 'Elli Ayak' when T.TruckType = 5 then 'T�r' when T.TruckType = 6 then 'Di�er' else '' end) as [Kamyon T�r�], D.NameSurname as [�of�r], D.LicenseNumber as [Ehliyet No], D.CellPhone as [Cep No] from TruckDriver D inner join Truck T on T.Oid = D.Truck where D.GCRecord is null