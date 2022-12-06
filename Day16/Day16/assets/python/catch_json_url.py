#使用requests 、lxml 套件
import requests
from lxml import  html
url = "https://data.gov.tw/dataset/31897"
res=requests.get(url)
#用lxml的html函式來處理內容格式
byte_data = res.content
source_code = html.fromstring(byte_data)
result = source_code.xpath("/html/head/script[1]/text()")[0]

out = result.find("JSON")
l_len = out+len('JSON","contentUrl":"')
r_len  = result[l_len:].find('"')+l_len
json_url = result[l_len:r_len]


htmls = requests.get(json_url,verify=False)
print(htmls.text)