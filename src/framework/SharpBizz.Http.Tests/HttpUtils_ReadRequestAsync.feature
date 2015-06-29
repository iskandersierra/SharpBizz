@httpSerialization
Feature: HttpUtils_ReadRequestAsync
	Create a HttpRequestMessage from a binary serialized Http request message

Scenario Outline: Read a simple request message
	Given A simple HTTP request is recieved with "<method>", "http://www.example.com/" and HTTP version 1 and 1
	When  The message is parsed as a HttpRequestMessage
	Then  The request is not null
	And   The request method is "<method>"
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

Scenario Outline: Read a simple request message with different types of URLs
	Given A simple HTTP request is recieved with "GET", "<url>" and HTTP version 1 and 1
	When  The message is parsed as a HttpRequestMessage
	Then  The request is not null
	And   The request url is "<url>"
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

Scenario Outline: Read a simple request message with different HTTP versions
	Given A simple HTTP request is recieved with "GET", "http://www.example.com/" and HTTP version <major> and <minor>
	When  The message is parsed as a HttpRequestMessage
	Then  The request is not null
	And   The request version is <major> and <minor>
	Examples:
	| major | minor |
	| 1     | 1     |
	| 1     | 0     |
	| 1     | 7     |
	| 2     | 0     |
	| 0     | 9     |

Scenario: Read a simple request message with no headers and no content
	Given A simple HTTP request is recieved with "GET", "http://www.example.com/" and HTTP version 1 and 1
	When  The message is parsed as a HttpRequestMessage
	Then  The request is not null
	And The request headers count is 0
	And The request content is null

Scenario Outline: Read a simple request message with one single-valued content header and no content
	Given A simple HTTP GET request is recieved with "<header>" with value "<value>"
	When  The message is parsed as a HttpRequestMessage
	Then  The request is not null
	And The request content headers count is 1
	And The request headers count is 0
	And The request content header "<header>" has value "<value>"
	Examples:
	| header              | value                            |
	| Content-Disposition | attachment; filename="fname.ext" |
	| Content-Location    | /index.htm                       |
	| Content-MD5         | Q2hlY2sgSW50ZWdyaXR5IQ==         |
	| Content-Range       | bytes 21010-47021/47022          |
	| Content-Type        | text/html; charset=utf-8         |
	| Expires			  | Thu, 01 Dec 1994 16:00:00 GMT    |
	| Last-Modified  	  | Tue, 15 Nov 1994 12:45:26 GMT    |

Scenario Outline: Read a simple request message with one multi-valued content header and no content
	Given A simple HTTP GET request is recieved with "<header>" with value "<values>"
	When  The message is parsed as a HttpRequestMessage
	Then  The request is not null
	And The request content headers count is 1
	And The request headers count is 0
	And The request content header "<header>" has values "<values>" with <count> elements
	Examples:
	| header           | values                  | count |
	| Allow            | GET, HEAD, OPTIONS      | 3     |
	| Content-Encoding | gzip, lzha              | 2     |
	| Content-Language | da, en-US, pt-PT, es-ES | 4     |

Scenario Outline: Read a simple request message with one single-valued request header and no content
	Given A simple HTTP GET request is recieved with "<header>" with value "<value>"
	When  The message is parsed as a HttpRequestMessage
	Then  The request is not null
	And The request content headers count is 0
	And The request headers count is 1
	And The request header "<header>" has value "<value>"
	Examples:
	| header              | value                                  |
	| Accept-Datetime     | Thu, 31 May 2007 20:35:00 GMT          |
	| Authorization       | Basic QWxhZGRpbjpvcGVuIHNlc2FtZQ==     |
	| Cache-Control       | no-cache                               |
	| Connection          | keep-alive                             |
	| Cookie              | $Version=1; Skin=new;                  |
	| Date                | Tue, 15 Nov 1994 08:12:31 GMT          |
	| Expect              | 100-continue                           |
	| From                | user@example.com                       |
	| Host                | www.example.com:80                     |
	| If-Match            | "737060cd8c284d8af7ad3082f209582d"     |
	| If-Modified-Since   | Tue, 15 Nov 1994 08:12:31 GMT          |
	| If-None-Match       | "737060cd8c284d8af7ad3082f209582d"     |
	| If-Range            | "737060cd8c284d8af7ad3082f209582d"     |
	| If-Unmodified-Since | Tue, 15 Nov 1994 08:12:31 GMT          |
	| Max-Forwards        | 10                                     |
	| Origin              | http://www.example-social-network.com  |
	| Pragma              | no-cache                               |
	| Proxy-Authorization | Basic QWxhZGRpbjpvcGVuIHNlc2FtZQ==     |
	| Range               | bytes=500-999                          |
	| Referer             | http://en.wikipedia.org/wiki/Main_Page |


Scenario Outline: Read a simple request message with one multi-valued header and no content
	Given A simple HTTP GET request is recieved with "<header>" with value "<values>"
	When  The message is parsed as a HttpRequestMessage
	Then  The request is not null
	And The request content headers count is 0
	And The request headers count is 1
	And The request header "<header>" has values "<values>" with <count> elements of type <valuesType>
	Examples:
	| header          | values                                                               | count | valuesType      |
	| Accept          | text/plain, text/html                                                | 2     | CommaSeparated  |
	| Accept-Charset  | utf-8, utf-16, utf-32                                                | 3     | CommaSeparated  |
	| Accept-Encoding | gzip, deflate                                                        | 2     | CommaSeparated  |
	| Accept-Language | en-US, es-ES, pt-PT                                                  | 3     | CommaSeparated  |
	| TE              | trailers, deflate                                                    | 2     | CommaSeparated  |
	| Upgrade         | HTTP/2.0, SHTTP/1.3, IRC/6.9, RTA/x11                                | 4     | CommaSeparated  |
	| User-Agent      | Mozilla/5.0 (X11; Linux x86_64; rv:12.0) Gecko/20100101 Firefox/21.0 | 4     | ProductComments |
	| Via             | 1.0 fred, 1.1 example.com (Apache/1.1)                               | 2     | CommaSeparated  |

Scenario Outline: Read a request message with single-line plain text content
	Given A HTTP POST request is recieved with single-line plain text "<content>"
	When  The message is parsed as a HttpRequestMessage
	Then  The request is not null
	And The request content headers count is 2
	And The request headers count is 0
	And The request content length matches "<content>" length
	Examples:
	| content        |
	| Hello world!!! |

Scenario Outline: Read a request message with binary content
	Given A HTTP POST request is recieved with byte values up to <count> bytes
	When  The message is parsed as a HttpRequestMessage
	Then  The request is not null
	And The request content headers count is 2
	And The request headers count is 0
	And The request content length is "<count>"
	Examples:
	| count |
	| 0     |
	| 1     |
	| 10    |
	| 100   |

