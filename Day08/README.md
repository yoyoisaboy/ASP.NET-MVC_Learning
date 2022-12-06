# ASP .NET MVC 上手 Day08(產品管理系統)

###### tags: `asp.netMVC` `C#`

## MVVM - Model View ViewModel
好處有:
1. 程式的階層分工更明確。
2. 減少程式進行與使用者介面的互動。
3. 可以根據設計期/執行決定不同的資料來源。
4. 注重再商業邏輯(ViewModel)。
5. ViewModel提供的資料型別與資料，更接近實際View。

### Model
描述資料實體(Entity)的類別(Class)，實作商業邏輯以及與資料存取相關功能。
### View 
使用者界面，使用者可以和ViewMdel溝通，以存取多個Model至於ViewModel中，同時減少再View上面進行運算或撰寫複雜程式。
## ViewModel
簡單的說ViewModel就是提供給View呈現資料用的Model。


---


# 產品類別與產品資料合併查詢

## Model創 windNorthModel 實體資料模型
![](https://i.imgur.com/amDtHC8.png)

APP_DATA建置資料庫，建完記得右鍵專案建置~

## Model創 ProductViewModel 類別
![](https://i.imgur.com/Hi0cNdp.png)

新增以下物件

```
public class ProductViewModel
{
    public int 產品編號 { get; set; }
    public int 特價 { get; set; }
    public string 產品 { get; set; }
    public string 類別名稱 { get; set; }
    public string 單位數量 { get; set; }
    public Nullable<decimal> 單價 { get; set; }

}
```

## 新增 Sample01Controller.cs

```
public ActionResult Index()
{
    NorthwindEntities db = new NorthwindEntities();
    var product = from c in db.產品類別
                  join p in db.產品資料
                  on c.類別編號 equals p.類別編號
                  select new ProductViewModel
                  {
                      產品編號 = p.產品編號,
                      類別名稱 = c.類別名稱,
                      產品 = p.產品,
                      單位數量 = p.單位數量,
                      單價 = p.單價,
                      特價 = (int)((double)p.單價 * 0.9)
                  };
    return View(product);
}
```
## 新增檢視 Index.cshtml
使用List範本
![](https://i.imgur.com/uQSoiwK.png)

```
@model IEnumerable<Day08.Models.ProductViewModel>

@{
    ViewBag.Title = "Index";
}

<h2>Index</h2>

<p>
    @Html.ActionLink("Create New", "Create")
</p>
<table class="table">
    <tr>
        <th>
            @Html.DisplayNameFor(model => model.產品編號)
        </th>
        <th>
            @Html.DisplayNameFor(model => model.類別名稱)
        </th>
        <th>
            @Html.DisplayNameFor(model => model.產品)
        </th>

        <th>
            @Html.DisplayNameFor(model => model.單位數量)
        </th>
        <th>
            @Html.DisplayNameFor(model => model.單價)
        </th>
        <th>
            @Html.DisplayNameFor(model => model.特價)
        </th>


        <th></th>
    </tr>

@foreach (var item in Model) {
    <tr>
        <td>
            @Html.DisplayFor(modelItem => item.產品編號)
        </td>
        <td>
            @Html.DisplayFor(modelItem => item.類別名稱)
        </td>
        <td>
            @Html.DisplayFor(modelItem => item.產品)
        </td>
        <td>
            @Html.DisplayFor(modelItem => item.單位數量)
        </td>
        <td>
            @Html.DisplayFor(modelItem => item.單價)
        </td>
        <td>
            @Html.DisplayFor(modelItem => item.特價)
        </td>
        <td>
            @Html.ActionLink("Edit", "Edit", new { id=item.產品編號 }) |
            @Html.ActionLink("Details", "Details", new { id=item.產品編號 }) |
            @Html.ActionLink("Delete", "Delete", new { id=item.產品編號 })
        </td>
    </tr>
}

</table>

```
:::warning
開頭要改成@model IEnumerable<Day08.Models.ProductViewModel>，用我們要用的欄位定義。
:::
## 完成
![](https://i.imgur.com/eOHBnPk.png)


# 一對多資料表查詢

## Models 新增 CategoryProductViewModel.cs
```
public class CategoryProductViewModel
{
    public List<產品類別> Category { get; set; }
    public List<產品資料> Product { get; set; }
}
```

## Controllers 新增 Sample02Controller.cs
把資料庫的東西，塞到 Models 裡，傳到前端。
```
public ActionResult Index(int cid = 1)
{
    NorthwindEntities db = new NorthwindEntities();
    CategoryProductViewModel vm = new CategoryProductViewModel();
    vm.Category = db.產品類別.ToList();
    vm.Product = db.產品資料.Where(m => m.類別編號 == cid).ToList();
    return View(vm);
}
```
接著建立檢視

## Views -> Sample02 -> Index.cshtml

![](https://i.imgur.com/OfvhuYH.png)

```
@model Day08.Models.CategoryProductViewModel

@{
    ViewBag.Title = "Index";
}

<h2>Index</h2>

<p>
    @Html.ActionLink("Create New", "Create")
</p>

<div class="row">
    <div class="col-sm-2">
        @foreach(var item in Model.Category)
        {
            <p>@Html.ActionLink(item.類別名稱,"Index",new { cid = item.類別編號 })</p>
        }
    </div>
    <div class="col-sm-10">
        <table class="table">
            <tr>
                <th>
                    @Html.DisplayNameFor(model => model.Product.FirstOrDefault().產品編號)
                </th>
                <th>
                    @Html.DisplayNameFor(model => model.Product.FirstOrDefault().產品)
                </th>
                <th>
                    @Html.DisplayNameFor(model => model.Product.FirstOrDefault().供應商編號)
                </th>
                <th>
                    @Html.DisplayNameFor(model => model.Product.FirstOrDefault().類別編號)
                </th>
                <th>
                    @Html.DisplayNameFor(model => model.Product.FirstOrDefault().單位數量)
                </th>
                <th>
                    @Html.DisplayNameFor(model => model.Product.FirstOrDefault().單價)
                </th>
                <th>
                    @Html.DisplayNameFor(model => model.Product.FirstOrDefault().庫存量)
                </th>
                <th>
                    @Html.DisplayNameFor(model => model.Product.FirstOrDefault().已訂購量)
                </th>
                <th>
                    @Html.DisplayNameFor(model => model.Product.FirstOrDefault().安全存量)
                </th>
                <th>
                    @Html.DisplayNameFor(model => model.Product.FirstOrDefault().不再銷售)
                </th>
                <th></th>
            </tr>

            @foreach (var item in Model.Product)
            {
                <tr>
                    <td>
                        @Html.DisplayFor(modelItem => item.產品編號)
                    </td>
                    <td>
                        @Html.DisplayFor(modelItem => item.產品)
                    </td>
                    <td>
                        @Html.DisplayFor(modelItem => item.供應商編號)
                    </td>
                    <td>
                        @Html.DisplayFor(modelItem => item.類別編號)
                    </td>
                    <td>
                        @Html.DisplayFor(modelItem => item.單位數量)
                    </td>
                    <td>
                        @Html.DisplayFor(modelItem => item.單價)
                    </td>
                    <td>
                        @Html.DisplayFor(modelItem => item.庫存量)
                    </td>
                    <td>
                        @Html.DisplayFor(modelItem => item.已訂購量)
                    </td>
                    <td>
                        @Html.DisplayFor(modelItem => item.安全存量)
                    </td>
                    <td>
                        @Html.DisplayFor(modelItem => item.不再銷售)
                    </td>

                </tr>
            }

        </table>

    </div>
</div>
```
:::warning
注意第一行的資料來源，是用 Day08.Models.CategoryProductViewModel。不用List<>是因為Model本身就是List，所以不用特別在宣一次。
:::


---

[Day09](https://hackmd.io/I_RfW7GXTFyIeySzDdswgg?both)









