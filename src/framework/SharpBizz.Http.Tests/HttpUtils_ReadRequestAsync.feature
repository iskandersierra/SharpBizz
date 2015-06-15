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
	| header          | value                              |
	| Accept-Datetime | Thu, 31 May 2007 20:35:00 GMT      |
	| Authorization   | Basic QWxhZGRpbjpvcGVuIHNlc2FtZQ== |
	| Cache-Control   | no-cache                           |

Scenario Outline: Read a simple request message with one multi-valued header and no content
	Given A simple HTTP GET request is recieved with "<header>" with value "<values>"
	When  The message is parsed as a HttpRequestMessage
	Then  The request is not null
	And The request content headers count is 0
	And The request headers count is 1
	And The request header "<header>" has values "<values>" with <count> elements
	Examples:
	| header          | values                        | count |
	| Accept          | text/plain, text/html         | 2     |
	| Accept-Charset  | utf-8, utf-16, utf-32         | 3     |
	| Accept-Encoding | gzip, deflate                 | 2     |
	| Accept-Language | en-US, es-ES, pt-PT           | 3     |
