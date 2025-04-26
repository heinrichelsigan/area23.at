## CqrFacade

both peer-2-peer ip socket sending and Soap webserice (sync and async) sending facade facility
provides all send and receive operations, except direct listening on local interface server socket

## CqrSettings

basic singleton for persiting runtime settings

## CqrException

an application specific exception, which also provides has a linked list to all former exceptions

## Msg

### CFile
basic filesystem file

### CContact
basic contact 

### CImage
basic image 

### CContent 
basic content message

### CChatRoom
Chatroom abstraction

### CType
<pre>	
	[Serializable]
    public enum CType
    {
        None	= 0,
        Json 	= 1,
        Xml 	= 2,
        Mime 	= 3,
        Raw 	= 4
    }
</pre>

### IMsgAble
basic interface for messages
<pre>
	public interface IMsgAble
    {
        CType MsgType { get; }        
        string Message { get; }
        string SerializedMsg { get; }

        string Hash { get; }
        string Md5Hash { get; }

        string ToJson();
        T FromJson<T>(string jsonText);
        string ToXml();
        T FromXml<T>(string xmlText);
    }
</pre>
	