# httphelper 框架手册 和 工具
http://www.sufeinet.com/thread-9989-1-1.html
http://tool.sufeinet.com/HttpHelper.aspx

# HTML解析插件（注意它是根据文档解析，并不是根据页面渲染解析的。所以获取xpath的时候，要对比一下源码）
https://github.com/zzzprojects/html-agility-pack

# 极验开发文档
http://jiyandoc.c2567.com/433012

# 永乐相关的链接

## 获取参数的地址
http://www.228.com.cn/ajax/registerBind?t=1510537468654

## 检测是否登录成功(超重点)
https://www.228.com.cn/ajax/isLogin

## 登录地址
http://www.228.com.cn/auth/login



目前任务，登陆之后设置地址

1、最大的问题在于，如何最快速的获取提交订单所需要的信息，最难的有
    - o['addressid']，这个是目前发现最快的方式是在收获地址【https://www.228.com.cn/deliveryAddress/deliveryaddress】中获取。
        - 如果没有的话，说明还没有默认地址。如果有的话，就需要替换了。
    - cityid，只要是【我的订单】中的每个页面都有的fconfigid 



2、确认是否有默认地址，如果有，则修改第一条，如果没有，则添加最小化信息
    - 如何确认有信息存在，
        - 如果不存在的话就添加。
        - 如果存在的话就修改。