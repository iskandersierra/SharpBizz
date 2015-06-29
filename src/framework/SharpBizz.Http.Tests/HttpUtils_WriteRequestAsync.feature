@httpSerialization
Feature: HttpUtils_WriteRequestAsync
	Create a binary serialized version of a given HttpRequestMessage

Scenario Outline: Write a simple request message
	Given A simple HTTP request is created with "<method>", "http://www.example.com/" and HTTP version 1 and 1
	When  The request message is written to a stream
	Then  The stream is not empty
	And   The string representation of the stream is a simple request message with "<method>", "http://www.example.com/" and HTTP version 1 and 1
	Examples:
	| method  |
	| GET     |
	| POST    |
	| PUT     |
	| DELETE  |
	| HEAD    |
	| PATCH   |
	| OPTIONS |
	| CUSTOM  |

Scenario Outline: Write a simple request message with different types of URLs
	Given A simple HTTP request is created with "GET", "<url>" and HTTP version 1 and 1
	When  The request message is written to a stream
	Then  The stream is not empty
	And   The string representation of the stream is a simple request message with "GET", "<url>" and HTTP version 1 and 1
	Examples:
	| url                                           |
	| http://www.example.com/                       |
	| https://www.example.com/                      |
	| http://localhost/                             |
	| https://localhost/                            |
	| http://192.168.17.1/                          |
	| https://192.168.17.1/                         |
	| http://www.example.com/path                   |
	| http://www.example.com/path.ext               |
	| http://www.example.com/?query=value           |
	| http://www.example.com/?query=value&other=123 |
	| http://www.example.com/path with escapes/     |

Scenario Outline: Write a simple request message with different HTTP versions
	Given A simple HTTP request is created with "GET", "http://www.example.com/" and HTTP version <major> and <minor>
	When  The request message is written to a stream
	Then  The stream is not empty
	And   The string representation of the stream is a simple request message with "GET", "http://www.example.com/" and HTTP version <major> and <minor>
	Examples:
	| major | minor |
	| 1     | 1     |
	| 1     | 0     |
	| 1     | 7     |
	| 2     | 0     |
	| 0     | 9     |

Scenario Outline: Write a simple request message with one content header and no content
	Given A simple request is created with content "<header>" with value "<value>" and empty string content
	When  The request message is written to a stream
	Then  The stream is not empty
	And The string representation of the stream is a simple request with "<header>" with value "<value>" and plain text content type
	Examples:
	| header              | value                            |
	| Content-Disposition | attachment; filename="fname.ext" |
	| Content-Location    | /index.htm                       |
	| Content-MD5         | Q2hlY2sgSW50ZWdyaXR5IQ==         |
	| Content-Range       | bytes 21010-47021/47022          |
	| Expires             | Thu, 01 Dec 1994 16:00:00 GMT    |
	| Last-Modified       | Tue, 15 Nov 1994 12:45:26 GMT    |
	| Allow               | GET, HEAD, OPTIONS               |
	| Content-Encoding    | gzip, lzha                       |
	| Content-Language    | da, en-US, pt-PT, es-ES          |

Scenario Outline: Write a simple request message with one request header and no content
	Given A simple request is created with request "<header>" with value "<value>"
	When  The request message is written to a stream
	Then  The stream is not empty
	And The string representation of the stream is a simple request with "<header>" with value "<value>"
	Examples:
	| header              | value                                                                |
	| Accept-Datetime     | Thu, 31 May 2007 20:35:00 GMT                                        |
	| Authorization       | Basic QWxhZGRpbjpvcGVuIHNlc2FtZQ==                                   |
	| Cache-Control       | no-cache                                                             |
	| Connection          | keep-alive                                                           |
	| Cookie              | $Version=1; Skin=new;                                                |
	| Date                | Tue, 15 Nov 1994 08:12:31 GMT                                        |
	| Expect              | 100-continue                                                         |
	| From                | user@example.com                                                     |
	| Host                | www.example.com:80                                                   |
	| If-Match            | "737060cd8c284d8af7ad3082f209582d"                                   |
	| If-Modified-Since   | Tue, 15 Nov 1994 08:12:31 GMT                                        |
	| If-None-Match       | "737060cd8c284d8af7ad3082f209582d"                                   |
	| If-Range            | "737060cd8c284d8af7ad3082f209582d"                                   |
	| If-Unmodified-Since | Tue, 15 Nov 1994 08:12:31 GMT                                        |
	| Max-Forwards        | 10                                                                   |
	| Origin              | http://www.example-social-network.com                                |
	| Pragma              | no-cache                                                             |
	| Proxy-Authorization | Basic QWxhZGRpbjpvcGVuIHNlc2FtZQ==                                   |
	| Range               | bytes=500-999                                                        |
	| Referer             | http://en.wikipedia.org/wiki/Main_Page                               |
	| Accept              | text/plain, text/html                                                |
	| Accept-Charset      | utf-8, utf-16, utf-32                                                |
	| Accept-Encoding     | gzip, deflate                                                        |
	| Accept-Language     | en-US, es-ES, pt-PT                                                  |
	| TE                  | trailers, deflate                                                    |
	| Upgrade             | HTTP/2.0, SHTTP/1.3, IRC/6.9, RTA/x11                                |
	| User-Agent          | Mozilla/5.0 (X11; Linux x86_64; rv:12.0) Gecko/20100101 Firefox/21.0 |
	| Via                 | 1.0 fred, 1.1 example.com (Apache/1.1)                               |

Scenario: Write a request message with byte array content
	Given A HTTP request is created with byte array content of 100 bytes
	When The request message is written to a stream
	Then  The stream is not empty
	And   The string representation of the stream is a request with a 100 bytes content
