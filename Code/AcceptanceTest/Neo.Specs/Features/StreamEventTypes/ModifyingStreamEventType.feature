Feature: Modifying stream event type

In order to registering and tracking life strams online
As me
I want to configure Stream event type

Scenario Outline: Stream event type gets modified with its valid properties
	Given There are some provided stream event type metadata with following properties
		| Title     |
		| Init Date |
	And There is a defined stream event type with following properties
		| Title | Metadata |
		| Init  |          |
	And I have reprovided some stream event type metadata with following properties
		| Title               |
		| <newMetadataTitle1> |
		| <newMetadataTitle2> |
	When I modify stream event type 'Init' with follwoing properties
		| Title      | Metadata |
		| <newTitle> |          |
	Then I can find stream event type '<newTitle>' with above properties

Examples:
	| newTitle                                                                                                                         | newMetadataTitle1 | newMetadataTitle2 |
	| Conversation                                                                                                                     | Duration          | IsClosed          |
	| ssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss | X                 | Z                 |

Scenario: Stream event type is not allowed to get modified without matadata
	Given There are some provided stream event type metadata with following properties
		| Title     |
		| Init Date |
	And There is a defined stream event type with following properties
		| Title | Metadata |
		| Init  |          |
	When I modify stream event type 'Init' with follwoing properties
		| Title      | Metadata |
		| <newTitle> |          |
	Then I get error with code 'NEO-SET-BR-10005' and message 'Stream event type metadata is required' from the system

Scenario Outline: Stream event type is not allowed to get modified with invalid properties
	Given There are some provided stream event type metadata with following properties
		| Title    |
		| InitDate |
	And There is a defined stream event type with following properties
		| Title | Metadata |
		| Init  |          |
	And I have reprovided some stream event type metadata with following properties
		| Title            |
		| <metadataTitle1> |
		| <metadataTitle2> |
	When I modify stream event type 'Init' with follwoing properties
		| Title   | Metadata |
		| <title> |          |
	Then I get error with code '<errorCode>' and message '<errorMessage>' from the system
		

Examples:
	| errorCode        | errorMessage                                                        | title                                                                                                                             | metadataTitle1                                                                                                                     | metadataTitle2 |
	| NEO-SET-BR-10002 | Stream event type title is required                                 |                                                                                                                                   | Init Date                                                                                                                          | X              |
	| NEO-SET-BR-10003 | Stream event type title can not include special characters          | $Init                                                                                                                             | Init Date                                                                                                                          | X              |
	| NEO-SET-BR-10003 | Stream event type title can not include special characters          | &Init                                                                                                                             | Init Date                                                                                                                          | X              |
	| NEO-SET-BR-10003 | Stream event type title can not include special characters          | #Init                                                                                                                             | Init Date                                                                                                                          | X              |
	| NEO-SET-BR-10003 | Stream event type title can not include special characters          | !Init                                                                                                                             | Init Date                                                                                                                          | X              |
	| NEO-SET-BR-10003 | Stream event type title can not include special characters          | [Init                                                                                                                             | Init Date                                                                                                                          | X              |
	| NEO-SET-BR-10003 | Stream event type title can not include special characters          | Init]                                                                                                                             | Init Date                                                                                                                          | X              |
	| NEO-SET-BR-10003 | Stream event type title can not include special characters          | (Init                                                                                                                             | Init Date                                                                                                                          | X              |
	| NEO-SET-BR-10003 | Stream event type title can not include special characters          | Init)                                                                                                                             | Init Date                                                                                                                          | X              |
	| NEO-SET-BR-10004 | Stream event type title length can not be greater than 128          | sssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss | Init Date                                                                                                                          | X              |
	| NEO-SET-BR-10006 | Stream event type metadata title is required                        | Init                                                                                                                              |                                                                                                                                    | X              |
	| NEO-SET-BR-10007 | Stream event type metadata title can not include special characters | Init                                                                                                                              | $Init Date                                                                                                                         | X              |
	| NEO-SET-BR-10007 | Stream event type metadata title can not include special characters | Init                                                                                                                              | Init%Date                                                                                                                          | X              |
	| NEO-SET-BR-10007 | Stream event type metadata title can not include special characters | Init                                                                                                                              | #Init Date                                                                                                                         | X              |
	| NEO-SET-BR-10007 | Stream event type metadata title can not include special characters | Init                                                                                                                              | !Init Date                                                                                                                         | X              |
	| NEO-SET-BR-10007 | Stream event type metadata title can not include special characters | Init                                                                                                                              | [Init Date                                                                                                                         | X              |
	| NEO-SET-BR-10007 | Stream event type metadata title can not include special characters | Init                                                                                                                              | Init Date]                                                                                                                         | X              |
	| NEO-SET-BR-10007 | Stream event type metadata title can not include special characters | Init                                                                                                                              | (Init Date                                                                                                                         | X              |
	| NEO-SET-BR-10007 | Stream event type metadata title can not include special characters | Init                                                                                                                              | Init Date)                                                                                                                         | X              |
	| NEO-SET-BR-10008 | Stream event type metadata title length can not be greater than 128 | Feeling                                                                                                                           | ssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss s | X              |

Scenario: Stream event type is not allowed to get modified with duplicated matadata
	Given There are some provided stream event type metadata with following properties
		| Title    |
		| Duration |
	And There is a defined stream event type with following properties
		| Title        | Metadata |
		| Conversation |          |
	And I have reprovided some stream event type metadata with following properties
		| Title    |
		| Duration |
		| Duration |
	When I modify stream event type 'Conversation' with follwoing properties
		| Title        | Metadata |
		| Conversation |          |
	Then I get error with code 'NEO-SET-BR-10009' and message 'There are duplicated metadata' from the system

	#TODO: comming soon
Scenario: Stream event type metadata is not allowed to get modified when an stream event is appended with