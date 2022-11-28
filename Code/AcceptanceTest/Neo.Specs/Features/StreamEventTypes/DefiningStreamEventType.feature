Feature: Defining Stream event type

In order to registering and tracking life strams online
As me
I want to configure stream event types

Scenario Outline: Stream event type gets defined with its valid properties
	When I define a new stream event type with following properties
		| Title   | Metadata |
		| <title> |          |
	Then I can find stream event type '<title>' with above properties

Examples:
	| title                                                                                                                            |
	| Init                                                                                                                             |
	| Feeling                                                                                                                          |
	| Guess                                                                                                                            |
	| Assumption                                                                                                                       |
	| Conversation                                                                                                                     |
	| Conclusion                                                                                                                       |
	| ssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss |

Scenario Outline: Stream event type gets defined with matadata and valid properties
	Given There are some provided stream event type metadata with following properties
		| Title            |
		| <metadataTitle1> |
		| <metadataTitle2> |
	When I define a new stream event type with following properties
		| Title   | Metadata |
		| <title> |          |
	Then I can find stream event type '<title>' with above properties

Examples:
	| title   | metadataTitle1                                                                                                                   | metadataTitle2 |
	| Init    | Init Date                                                                                                                        | X              |
	| Feeling | ssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss | X              |

Scenario: Stream event type gets defined with its unique properties
	Given There is a defined stream event type with following properties
		| Title | Metadata |
		| Init  |          |
	When I define a new stream event type with follwoing properties
		| Title | Metadata |
		| Init  |          |
	Then I get error with code 'NEO-LSE-BR-10001' and message 'There is already a registered stream event type with title "Init"' within the system

Scenario Outline: Stream event type is not allowed to get defined with invalid properties
	Given There are some provided stream event type metadata with following properties
		| Title            |
		| <metadataTitle1> |
		| <metadataTitle2> |
	When I define a new stream event type with following properties
		| Title   | Metadata |
		| <title> |          |
	Then I get error with code '<errorCode>' and message '<errorMessage>' from the system

Examples:
	| errorCode        | errorMessage                                                        | title                                                                                                                             | metadataTitle1                                                                                                                     | metadataTitle2 |
	| NEO-SET-BR-10002 | Stream event type title is required                                 |                                                                                                                                   | Init Date                                                                                                                          | X              |
	| NEO-SET-BR-10003 | Stream event type title can not include invalid character           | $Init                                                                                                                             | Init Date                                                                                                                          | X              |
	| NEO-SET-BR-10004 | Stream event type title length can not be greater than 128          | sssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss | Init%Date                                                                                                                          | X              |
	| NEO-SET-BR-10005 | Stream event type metadata title is required                        | Init                                                                                                                              |                                                                                                                                    | X              |
	| NEO-SET-BR-10006 | Stream event type metadata title can not include special character  | Init                                                                                                                              | $Init Date                                                                                                                         | X              |
	| NEO-SET-BR-10007 | Stream event type metadata title length can not be greater than 128 | Feeling                                                                                                                           | ssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss s | X              |

Scenario Outline: Stream event type is not allowed to get defined with duplicated matadata
	Given There are some provided stream event type metadata with following properties
		| Title    |
		| Duration |
		| Duration |
	When I define a new stream event type with following properties
		| Title        | Metadata |
		| Conversation |          |
	Then I get error with code 'NEO-SET-BR-10008' and message 'There are duplicated metadata' from the system