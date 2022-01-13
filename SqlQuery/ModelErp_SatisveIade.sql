--select * from sts_fatura where musteri_no='120.06.01.0004' and r_sayac='12785'
--select * from sts_fatura_detay where fatura_rsayac='12785'

select SUM(d.miktari) as satis_miktar, SUM(f.tutar_toplam) as satis_tutar,
	(select sum(miktar) as miktar from mlz_yerlestirme_listesi_satir 
		where kod = f.musteri_no and YEAR(r_tarih) = 2015 and MONTH(r_tarih) = 8) as iade_miktar
from sts_fatura f inner join sts_fatura_detay d on d.fatura_rsayac = f.r_sayac 
where f.musteri_no = '120.08.01.0005' and YEAR(f.hazirlama_tarihi) = 2015 and MONTH(f.hazirlama_tarihi) = 8
group by f.musteri_no

--select sum(miktar) as miktar from mlz_yerlestirme_listesi_satir where kod = '120.06.01.0004' and MONTH(r_tarih) = 8