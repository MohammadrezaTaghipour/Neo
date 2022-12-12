Feature: Modifying stream Context

In order to registering and tracking life strams online
As me
I want to configure stream contexts

Scenario Outline: Stream context gets modified with its valid properties
	Given There are some defined stream event type with following properties
		| Title        | Metadata |
		| Conversation |          |
		| Conclusion   |          |
	And there is a provided stream context with following properties
		| Title       | Description                                                                                            |
		| Career path | this stream context includes all career-related stuffs such as my colleges and companies I worked for. |
	And With following stream event type
		| StreamEventType |
		| Conversation    |
	And There is a defined stream context 'Career path'
	When I reprovide stream context 'Career path' with following properties
		| Title      | Description |
		| My Familiy |             |
	And With following stream event type
		| StreamEventType |
		| Conclusion      |
	And I modify stream context 'Career path'
	Then I can find stream context 'Career path' with above properties

Scenario Outline: Stream context is not allowed to get modified with invalid properties
	Given There is a defined stream event type with following properties
		| Title        | Metadata |
		| Conversation |          |
	And there is a provided stream context with following properties
		| Title       | Description |
		| Career path |             |
	And With following stream event type
		| StreamEventType |
		| Conversation    |
	And There is a defined stream context 'Career path'
	When I reprovide stream context 'Career path' with following properties
		| Title   | Description   |
		| <title> | <description> |
	And With following stream event type
		| StreamEventType   |
		| <streamEventType> |
	And I modify stream context 'Career path'
	Then I get error with code '<errorCode>' and message '<errorMessage>' from the system

Examples:
	| errorCode       | errorMessage                                                       | title                                                                                                                                                                                                                                                             | streamEventType |
	| NEO-SC-BR-10002 | Stream context title is required                                   |                                                                                                                                                                                                                                                                   | Conversation    |
	| NEO-SC-BR-10003 | Stream context title can not include special characters            | $Career                                                                                                                                                                                                                                                           | Conversation    |
	| NEO-SC-BR-10003 | Stream context title can not include special characters            | &Career                                                                                                                                                                                                                                                           | Conversation    |
	| NEO-SC-BR-10003 | Stream context title can not include special characters            | #Career                                                                                                                                                                                                                                                           | Conversation    |
	| NEO-SC-BR-10003 | Stream context title can not include special characters            | !Career                                                                                                                                                                                                                                                           | Conversation    |
	| NEO-SC-BR-10003 | Stream context title can not include special characters            | [Career                                                                                                                                                                                                                                                           | Conversation    |
	| NEO-SC-BR-10003 | Stream context title can not include special characters            | Career]                                                                                                                                                                                                                                                           | Conversation    |
	| NEO-SC-BR-10003 | Stream context title can not include special characters            | (Career                                                                                                                                                                                                                                                           | Conversation    |
	| NEO-SC-BR-10003 | Stream context title can not include special characters            | Career)                                                                                                                                                                                                                                                           | Conversation    |
	| NEO-SC-BR-10004 | Stream context title length can not be greater than 128            | sssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss                                                                                                                                 | Conversation    |
	| NEO-SC-BR-10005 | Stream context description length can not be greater than 256      | sssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss | Conversation    |
	| NEO-SC-BR-10006 | Stream context can not be created due to invalid stream event type | Career                                                                                                                                                                                                                                                            |                 |
	
Scenario: Stream context can not be modified when stream event type is not found
	Given There is a defined stream event type with following properties
		| Title        | Metadata |
		| Conversation |          |
	And Stream event type 'Conversation' has been removed
	When I reprovide stream context 'Career path' with following properties
		| Title      | Description |
		| My Familiy |             |
	And With following stream event type
		| StreamEventType |
		| Conversation    |
	And I modify stream context 'Career path'
	Then I get error with code 'NEO-SC-BR-10006' and message 'Stream context can not be created due to invalid stream event type' from the system

Scenario Outline: Stream context is not allowed to get modified with duplicated stream event type
	Given There are some defined stream event type with following properties
		| Title        | Metadata |
		| Conversation |          |
		| Conclusion   |          |
	When I reprovide stream context 'Career path' with following properties
		| Title      | Description |
		| My Familiy |             |
	And With following stream event type
		| StreamEventType |
		| Conversation    |
		| Conclusion      |
	And I modify stream context 'Career path'
	Then I get error with code 'NEO-SC-BR-10007' and message 'There are duplicated strean event types' from the system