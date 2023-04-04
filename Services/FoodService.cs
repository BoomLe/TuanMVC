using App.Models;

namespace App.Services
{
    public class FoodService : List<FoodModel>
    {
        public FoodService()
        {
            Add(new FoodModel()
            {
                Id = 1,
                Name ="Sushi",
                Country = "Nhật Bản",
                Description= @"Được chế biến từ gạo ngâm giấm và nhiều loại nguyên liệu 
                bao gồm hải sản, rau và đôi khi là trái cây, sushi ngon nhất khi ăn kèm với 
                mù tạt, gừng ngâm chua và nước tương. Một loại trang trí phổ biến cho món ăn 
                này là củ cải Daikon. Loại cá trong sushi quyết định hương vị của món ăn này.
                Bên cạnh đó, cơm trộn giấm mang lại cho món ăn một hương vị tổng thể khá hấp dẫn.
                Cá ngừ, lươn và cá hồi có xu hướng có hương vị nhẹ trong khi sushi hương 
                bạch tuộc thường có hương vị mạnh và khá kén người ăn."

            });

              Add(new FoodModel()
            {
                Id = 2,
                Name ="Rendang",
                Country ="Indonesia",
                Description= @"Rendang được chế biến bằng cách ninh thịt bò với nước cốt dừa 
                với hỗn hợp các loại gia vị bao gồm nghệ, tỏi, sả, gừng, ớt và riềng. 
                Món ăn sau đó được hầm trong vài giờ để tạo cho nó một kết cấu mềm và hương vị 
                kỳ lạ. Sự bùng nổ của hương vị chắc chắn là một trong những lý do tại sao món ăn 
                này được yêu thích trên toàn cầu và cũng là một trong những món ăn ngon nhất 
                trên thế giới. Dễ xào nên món này thường được dùng trong các buổi lễ hoặc 
                đãi khách."

            });

              Add(new FoodModel()
            {
                Id = 3,
                Name ="Ramen ",
                Country = "Nhật Bản",
                Description=@"Ramen là một món ăn Nhật Bản bao gồm mì, nước dùng cùng với các 
                loại rau và thịt. Ramen có nhiều hương vị, từ cay đến không cay, tùy thuộc vào 
                hương vị của nước dùng. Mỗi vùng ở Nhật Bản đều có một công thức nước dùng cho 
                ramen riêng biệt. Hai loại mì ramen phổ biến nhất là Tonkotsu và Miso ramen."

            });

              Add(new FoodModel()
            {
                Id = 4,
                Name ="Kebab",
                Country = "Thổ Nhĩ Kỳ",
                Description=@"Là một món ăn phổ biến trên khắp Trung Đông, Kebabs có nguồn gốc 
                từ Thổ Nhĩ Kỳ. Chúng bao gồm thịt xay hoặc hải sản, trái cây và rau trong một 
                số phiên bản và được bằng than. Loại thịt trong món ăn này chủ yếu là thịt cừu được 
                ướp với tỏi, hạt tiêu đen và dầu thực vật. Nhiều nơi khác cũng sử dụng công thức 
                tương tự với bò, dê, cá và gà. Món ăn này có sự pha trộn hoàn hảo của hương vị, 
                từ thơm đến cay, khiến nó trở thành một trong những món ăn ngon nhất trên thế giới."

            });

              Add(new FoodModel()
            {
                Id = 5,
                Name ="Pho",
                Country = "Việt Nam",
                Description=@"Không cần phải nói quá nhiều về món ăn đặc trưng này của Việt Nam. 
                Phở có hương vị thơm ngon với sợi phở được làm từ gạo, ăn kèm nước hầm xương và 
                thịt bò, gà, kết hợp với nhiều loại gia vị tạo nên một món ăn mà bất cứ du khách 
                nào khi ghé thăm Việt Nam đều không muốn bỏ lỡ."

            });

              Add(new FoodModel()
            {
                Id = 6,
                Name ="PekingDuck",
                Country = "Trung Quốc",
                Description=@"Những con vịt cho món ăn này được nuôi và giết mổ đặc biệt sau 60 ngày và được tẩm gia vị trước khi quay trong lò kín, 
                điều này giúp thịt có lớp da giòn và mỏng hơn. Món ăn được phục vụ với dưa chuột, 
                hành lá và nước sốt đậu ngọt. Vịt quay Bắc Kinh sẽ được cắt trước mặt thực khách và sau 
                đó được phục vụ với nhiều công thức khác tiếp sau như: ăn cùng với nước sốt tỏi và bánh kếp 
                hay ninh lấy nước dùng."

            });

              Add(new FoodModel()
            {
                Id = 7,
                Name ="Paella",
                Country = "Tây Ban Nha",
                Description=@"Paella có nguồn gốc từ Valencia, Tây Ban Nha. Đây là một món ăn 
                cổ xưa được tái hiện lại với nét hiện đại trong thời hiện tại. Có nhiều cách khác
                nhau để ăn Paella. Công thức ban đầu bao gồm cơm trắng với đậu xanh, thịt 
                (thỏ hoặc gà, đôi khi vịt), đậu nành, ốc, phủ thêm hương thảo 
                (có thể thay hương thảo bằng atisô khi vào mùa)."

            });

              Add(new FoodModel()
            {
                Id = 8,
                Name ="Goulash",
                Country ="Hungary",
                Description=@"Goulash là món thịt hầm có từ thế kỷ thứ 9 ở Hungary. 
                Yếu tố chính làm nên món ăn là các loại gia vị, đặc biệt là ớt bột. 
                Goulash được chế biến từ thịt bò, thịt lợn, thịt bê hoặc thịt cừu. Thịt được 
                cắt thành từng miếng vừa ăn, ướp gia vị muối. Sau đó, nó được rang vàng với dầu 
                và hành tây cắt lát. Món ăn được để lửa nhỏ sau khi thêm ớt bột vào. 
                Nó được phục vụ với nhiều loại rau bao gồm cà rốt, mùi tây, cần tây và khoai tây. 
                Khoai tây cũng được cho vào món ăn để làm dày và mịn hơn (vì nhiều tinh bột). 
                Món ăn là biểu tượng cho đất nước Hungary và cũng là một trong những món ăn ngon 
                nhất thế giới.

"

            });

              Add(new FoodModel()
            {
                Id = 9,
                Name ="Lasagna",
                Country = "Ý",
                Description=@"Món lasagna của Ý đã vượt qua Pizza để được thêm vào danh sách 
                những món ăn ngon nhất thế giới vì sự trở lại của nó. Đây là một trong những 
                món mì lâu đời nhất nhưng chỉ mới trở nên phổ biến trong những năm gần đây. 
                Lasagna được mọi người ở mọi lứa tuổi yêu thích và là món ăn lý tưởng cho bất kỳ 
                dịp lễ nào."

            });

              Add(new FoodModel()
            {
                Id = 10,
                Name ="Dosa",
                Country= "Ấn Độ",
                Description=@"Ấn Độ có rất nhiều đóng góp khi nói đến ẩm thực và dosa là món 
                ăn tốt nhất để đại diện cho đất nước này. Nói một cách dễ hiểu, dosa là một loại 
                bánh kếp được làm từ bột gạo lên men. Trước đó, Dosa chỉ nổi bật ở Nam Ấn Độ và 
                Sri Lanka. Bây giờ nó là một món ăn nổi tiếng trên toàn thế giới. 
                Dosa được phục vụ cùng với tương ớt và sambar (rau hầm)."

            });

            
        }

    }
    
}