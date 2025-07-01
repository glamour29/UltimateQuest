using UnityEngine;

public class ChiSo_ThucThe : MonoBehaviour
{
    public NhomTaiNguyenChiSo tainguyen;
    public NhomChiSoChinh chiSoChinh;
    public LoaiChiSoTanCong congTanCong;
    public LoaiChiSoPhongThu phongthu;


    public float LaySatThuongNguyenTo (out LoaiNguyenTo nguyento, float tiLePhongTo = 1)
    {
        float satThuongLua = congTanCong.SatThuongLua.layDuLieu();
        float satThuongBang= congTanCong.SatThuongBang.layDuLieu();
        float satThuongSet = congTanCong.SatThuongSet.layDuLieu();
        float bonusSatThuongNguyenTo = chiSoChinh.TriTue.layDuLieu(); //bonus elemental damage from intelligence +1 per INIT

        float satThuongCaoNhat = satThuongLua;
        nguyento = LoaiNguyenTo.Lua;

        if (satThuongBang > satThuongCaoNhat)
        {
            satThuongCaoNhat = satThuongBang;
            nguyento = LoaiNguyenTo.Bang;
        }

        if (satThuongSet > satThuongCaoNhat)
        {
            satThuongCaoNhat = satThuongSet;
            nguyento = LoaiNguyenTo.Set;
        }

        if(satThuongCaoNhat <= 0)
        {
            nguyento = LoaiNguyenTo.KhongCo;
            return 0;
        }

        float bonusLua = (satThuongLua == satThuongCaoNhat) ? 0 : satThuongLua * .5f; 
        float bonusBang = (satThuongBang == satThuongCaoNhat) ? 0 : satThuongBang * .5f; 
        float bonusSet = (satThuongSet == satThuongCaoNhat) ? 0 : satThuongSet * .5f;

        float satThuongLenNguyenToYeu = bonusLua + bonusBang + bonusSet;
        float satThuongCuoiCung = satThuongCaoNhat + satThuongLenNguyenToYeu + bonusSatThuongNguyenTo;

        return satThuongCuoiCung * tiLePhongTo;
    }

    public float TinhSatThuongVatLy(out bool BaoKich, float tiLePhongTo = 1 ) 
    {
        // Lấy sát thương cơ bản và cộng thêm từ sức mạnh
        float satThuongCoBan = congTanCong.SatThuong.layDuLieu();
        float satThuongCongThem = chiSoChinh.SucManh.layDuLieu();
        float tongSatThuongCoBan = satThuongCoBan + satThuongCongThem;

        // Lấy tỉ lệ bạo kích cơ bản và cộng thêm từ nhanh nhẹn (+0.3% mỗi điểm AGI)
        float tiLeBaoKichCoBan = congTanCong.TiLeBaoKich.layDuLieu();
        float tiLeBaoKichCongThem = chiSoChinh.NhanhNhen.layDuLieu() * 0.3f;
        float tongTiLeBaoKich = tiLeBaoKichCoBan + tiLeBaoKichCongThem;

        // Lấy sức mạnh bạo kích cơ bản và cộng thêm từ sức mạnh (+0.5% mỗi điểm STR)
        float sucManhBaoKichCoBan = congTanCong.SatThuongBaoKich.layDuLieu();
        float sucManhBaoKichCongThem = chiSoChinh.SucManh.layDuLieu() * 0.5f;
        float tongSucManhBaoKich = (sucManhBaoKichCoBan + sucManhBaoKichCongThem) / 100;
        // e.g. 150 / 100 = 1.5 (hệ số nhân)

        // Xác định xem có phải bạo kích không
        BaoKich = Random.Range(0, 100) < tongTiLeBaoKich;

        // Tính sát thương cuối cùng
        float satThuongCuoiCung = BaoKich ? tongSatThuongCoBan * tongSucManhBaoKich : tongSatThuongCoBan;

        return satThuongCuoiCung * tiLePhongTo;
    }

    public float LayKhangNguyenTo(LoaiNguyenTo nguyento)
    {
        float khangCoBan = 0;
        float bonusKhang = chiSoChinh.TriTue.layDuLieu() * .5f; //bonus khang tu tri tue 0.5% cho moi VIT

            switch (nguyento)
        {
            case LoaiNguyenTo.Lua:
                khangCoBan = phongthu.KhangLua.layDuLieu();
            break;
            case LoaiNguyenTo.Bang:
                khangCoBan = phongthu.KhangBang.layDuLieu();
                break;
            case LoaiNguyenTo.Set:
                khangCoBan = phongthu.KhangSet.layDuLieu();
                break;
        }
        float khang = khangCoBan + bonusKhang;
        float gioiHanKhang = 75f; // resistance will be capped at 75%
        float khangCuoiCung = Mathf.Clamp (khang,0,gioiHanKhang)/100 ;//convert value into 0 to 1 multiplier
        return khangCuoiCung;
    }

    public float layTiLeGiamSatThuongTuGiap(float GiaTriGiamSatThuong)
    {
        float giapGoc = phongthu.Giap.layDuLieu();
        float giapCongThem = chiSoChinh.TheLuc.layDuLieu();// Giáp cộng thêm từ thể lực: +1 mỗi điểm Thể Lực (VIT)
        float tongGiap = giapGoc + giapCongThem;

        float heSoGiamSatThuong =Mathf.Clamp (1 - GiaTriGiamSatThuong,0,1);
        float giapHieuQua = tongGiap * heSoGiamSatThuong;


        float tiLeGiamSatThuong = giapHieuQua / (giapHieuQua + 100);
        float gioiHanTiLeGiamSatThuong = .85f;


        //Mathf.Clamp(giá_trị, giá_trị_thấp_nhất, giá_trị_cao_nhất); // Giới hạn giá trị trong khoảng từ min đến max
        float tiLeGiamSatThuongCuoi = Mathf.Clamp(tiLeGiamSatThuong, 0, gioiHanTiLeGiamSatThuong);

        return tiLeGiamSatThuongCuoi;
    }

    public float layGiaTriGiamSatThuongTuGiap()
    {
        float GiaTriGiamSatThuongCuoiCung = congTanCong.GiaTriGiamSatThuongTuGiap.layDuLieu() / 100;
        return GiaTriGiamSatThuongCuoiCung;
    }

    // Tính máu tối đa cuối cùng (gốc + cộng thêm từ thể lực)
    public float layHpToiDa()
    {
        float HpToiDaGoc = tainguyen.sucKhoeToiDa.layDuLieu();
        float CongThemHpToiDa = chiSoChinh.TheLuc.layDuLieu() * 5;
        float HpToiDaCuoiCung = HpToiDaGoc + CongThemHpToiDa;

        return HpToiDaCuoiCung;
    }

    // Tính tỉ lệ né đòn cuối cùng (gốc + cộng thêm từ nhanh nhẹn, giới hạn tối đa)
    public float LayGiaTriNeDon()
    {
        float ChiSoNeDonGoc = phongthu.NeDon.layDuLieu();
        float bonusChiSoNeDon = chiSoChinh.NhanhNhen.layDuLieu() * .5f; //mỗi điểm nhanh nhẹn sẽ cho bạn 0.5% né đòn

        float TongChiSoNeDon = ChiSoNeDonGoc + bonusChiSoNeDon;
        float GioiHanNeDon = 80f;

        float NeDonCuoiCung = Mathf.Clamp(TongChiSoNeDon, 0, GioiHanNeDon);

        return NeDonCuoiCung;

    }

}
