using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


  public  class MessageProtocol
    {
    public const int FRIEND = 0; //好友相关
    public const int GROUP = 1; //群组相关
    public const int CHAT = 2; //聊天相关

    public const int ADD_FRIEND_CREQ = 10;//申请添加好友
    public const int ADD_FRIEND_SRES = 11;//添加好友的响应
    public const int ONE_ADD_YOU_SRES = 12;//有人加你好友
    public const int AGREE_ADD_FRIEND_CREQ = 13;//同意加好友
    public const int AGREE_ADD_FRIEND_SRES = 14;//同意加好友的响应
    public const int ONE_AGREED_YOU = 15;        //别人同意了你的申请
    public const int DELETE_FRIEND_CREQ = 16;//删除好友
    public const int DELETE_FRIEND_SRES = 17;//删除好友的响应
    public const int YOU_BE_DELETED = 18;//你被删除好友了

    public const int CREATE_GROUP_CREQ = 30; //创建群
    public const int CREATE_GROUP_SRES = 31;//建群的响应
    public const int ADD_GROUP_CREQ = 32;//申请入群
    public const int ADD_GROUP_SRES = 33;//申请响应
    public const int ONE_WANT_ADD_GROUP_SRES = 34;//有人申请入群
    public const int AGREE_ADD_GROUP_CREQ = 35;//群主同意申请入群
    public const int AGREE_ADD_GROUP_SRES = 36;//群主同意申请入群的响应
    public const int YOU_BE_AGREED_ENTER_GROUP = 37;//你被同意入群
    public const int QUIT_GROUP_CREQ = 38;//退出一个群
    public const int QUIT_GROUP_SRES = 39;//退出一个群的响应

    public const int CHAT_ME_TO_FRIEND_CREQ = 100;//和好友聊天
    public const int CHAT_ME_TO_FRIEND_SRES = 101;//和好友聊天的响应
    public const int CHAT_FRIEND_TO_ME_SRES = 102;//好友和我聊天

    public const int CHAT_ME_TO_GROUP = 110;
    public const int CHAT_GROUP_TO_ME = 111;


}

