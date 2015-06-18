@httpSerialization
Feature: HttpUtils_ReadResponseAsync
	Create a HttpResponseMessage from a binary serialized Http response message

Scenario Outline: Read a simple response message
	Given A simple HTTP response is recieved with "<status>", "<reason>" and HTTP version 1 and 1
	When  The message is parsed as a HttpResponseMessage
	Then  The response is not null
	And   The response status code is "<status>"
	And   The response reason phrase is "<reason>"
	Examples:
	| status | reason                |
	| 100    | Continue              |
	| 200    | OK                    |
	| 201    | Created               |
	| 301    | Moved Permanently     |
	| 404    | Not Found             |
	| 500    | Internal Server Error |

Scenario Outline: Read a simple response message with different HTTP versions
	Given A simple HTTP response is recieved with "200", "OK" and HTTP version <major> and <minor>
	When  The message is parsed as a HttpResponseMessage
	Then  The response is not null
	And   The response version is <major> and <minor>
	Examples:
	| major | minor |
	| 1     | 1     |
	| 1     | 0     |
	| 1     | 7     |
	| 2     | 0     |
	| 0     | 9     |

Scenario Outline: Read a simple response message with one single-valued content header and no content
	Given A simple HTTP response is recieved with "<header>" with value "<value>"
	When The message is parsed as a HttpResponseMessage
	Then  The response is not null
	And The response content headers count is 1
	And The response headers count is 0
	And The response content header "<header>" has value "<value>"
	Examples:
	| header              | value                            |
	| Content-Disposition | attachment; filename="fname.ext" |
	| Content-Location    | /index.htm                       |
	| Content-MD5         | Q2hlY2sgSW50ZWdyaXR5IQ==         |
	| Content-Range       | bytes 21010-47021/47022          |
	| Content-Type        | text/html; charset=utf-8         |
	| Expires			  | Thu, 01 Dec 1994 16:00:00 GMT    |
	| Last-Modified  	  | Tue, 15 Nov 1994 12:45:26 GMT    |

Scenario Outline: Read a simple response message with one multi-valued content header and no content
	Given A simple HTTP response is recieved with "<header>" with value "<values>"
	When The message is parsed as a HttpResponseMessage
	Then  The response is not null
	And The response content headers count is 1
	And The response headers count is 0
	And The response content header "<header>" has values "<values>" with <count> elements
	Examples:
	| header           | values                  | count |
	| Allow            | GET, HEAD, OPTIONS      | 3     |
	| Content-Encoding | gzip, lzha              | 2     |
	| Content-Language | da, en-US, pt-PT, es-ES | 4     |

Scenario Outline: Read a simple response message with one single-valued response header and no content
	Given A simple HTTP response is recieved with "<header>" with value "<value>"
	When The message is parsed as a HttpResponseMessage
	Then  The response is not null
	And The response content headers count is 0
	And The response headers count is 1
	And The response header "<header>" has value "<value>"
	Examples:
	| header                      | value                                                                                                                      |
	| Access-Control-Allow-Origin | *                                                                                                                          |
	| Accept-Patch                | text/example;charset=utf-8                                                                                                 |
	| Accept-Ranges               | bytes                                                                                                                      |
	| Age                         | 12                                                                                                                         |
	| Connection                  | close                                                                                                                      |
	| Cache-Control               | max-age=3600                                                                                                               |
	| Date                        | Tue, 15 Nov 1994 08:12:31 GMT                                                                                              |
	| ETag                        | "737060cd8c284d8af7ad3082f209582d"                                                                                         |
	| Link                        | </feed>; rel="alternate"                                                                                                   |
	| Location                    | http://www.w3.org/pub/WWW/People.html                                                                                      |
	| P3P                         | CP="This is not a P3P policy! See http://www.google.com/support/accounts/bin/answer.py?hl=en&answer=151657 for more info." |
	| Pragma                      | no-cache                                                                                                                   |
	| Proxy-Authenticate          | Basic                                                                                                                      |
	| Public-Key-Pins             | max-age=2592000; pin-sha256="E9CZ9INDbd+2eRQozYqqbQ2yXLVKB9+xcprMF+44U1g=";                                                |
	| Refresh                     | 5; url=http://www.w3.org/pub/WWW/People.html                                                                               |
	| Retry-After                 | 120                                                                                                                        |
	| Retry-After                 | Fri, 07 Nov 2014 23:59:59 GMT                                                                                              |
	| Set-Cookie                  | UserID=JohnDoe; Max-Age=3600; Version=1                                                                                    |
	| Strict-Transport-Security   | max-age=16070400; includeSubDomains                                                                                        |
	| Trailer                     | Max-Forwards                                                                                                               |
	| Transfer-Encoding           | gzip                                                                                                                       |

Scenario Outline: Read a simple response message with one multi-valued header and no content
	Given A simple HTTP response is recieved with "<header>" with value "<values>"
	When The message is parsed as a HttpResponseMessage
	Then  The response is not null
	And The response content headers count is 0
	And The response headers count is 1
	Examples:
	| header | values                                     | count |
	| Server | Apache/2.4.1 (Unix)                        | 2     |
	| TE     | chunked, compress, deflate, gzip, identity | 1     |
