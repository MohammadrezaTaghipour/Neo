﻿Feature: Modifying stream Context

In order to registering and tracking life strams online
As me
I want to configure stream contexts

Background:
	Given There are some provided stream event type metadata with following properties
		| Title       |
		| Description |
		| HappenedOn  |
	And There are some defined stream event types with following properties
		| Title        | Metadata |
		| Conversation |          |
		| Conclusion   |          |
	And there is a provided stream context with following properties
		| Title       | Description                                                                                            |
		| Career path | this stream context includes all career-related stuffs such as my colleges and companies I worked for. |
	And With following stream event types
		| StreamEventType |
		| Conversation    |
	And There is a defined stream context 'Career path'

Scenario: Stream context gets modified with its valid properties
	When I reprovide stream context 'Career path' with following properties
		| Title      | Description |
		| My Familiy |             |
	And I reprovide the following stream event types
		| StreamEventType |
		| Conclusion      |
	And I modify stream context 'Career path'
	Then I can find stream context 'Career path' with above properties

Scenario Outline: Stream context is not allowed to get modified with invalid properties
	When I reprovide stream context 'Career path' with following properties
		| Title   | Description   |
		| <title> | <description> |
	And I reprovide the following stream event types
		| StreamEventType   |
		| <streamEventType> |
	And I modify stream context 'Career path'
	Then I get error with code '<errorCode>' and message '<errorMessage>' from the system

Examples:
	| errorCode       | errorMessage                                                      | title                                                                                                                             | description                                                                                                                                                                                                                                                       | streamEventType |
	| NEO-SC-BR-10002 | Stream context title is required                                  |                                                                                                                                   |                                                                                                                                                                                                                                                                   | Conversation    |
	| NEO-SC-BR-10003 | Stream context title can not include special characters           | $Career                                                                                                                           |                                                                                                                                                                                                                                                                   | Conversation    |
	| NEO-SC-BR-10003 | Stream context title can not include special characters           | &Career                                                                                                                           |                                                                                                                                                                                                                                                                   | Conversation    |
	| NEO-SC-BR-10003 | Stream context title can not include special characters           | #Career                                                                                                                           |                                                                                                                                                                                                                                                                   | Conversation    |
	| NEO-SC-BR-10003 | Stream context title can not include special characters           | !Career                                                                                                                           |                                                                                                                                                                                                                                                                   | Conversation    |
	| NEO-SC-BR-10003 | Stream context title can not include special characters           | [Career                                                                                                                           |                                                                                                                                                                                                                                                                   | Conversation    |
	| NEO-SC-BR-10003 | Stream context title can not include special characters           | Career]                                                                                                                           |                                                                                                                                                                                                                                                                   | Conversation    |
	| NEO-SC-BR-10003 | Stream context title can not include special characters           | (Career                                                                                                                           |                                                                                                                                                                                                                                                                   | Conversation    |
	| NEO-SC-BR-10003 | Stream context title can not include special characters           | Career)                                                                                                                           |                                                                                                                                                                                                                                                                   | Conversation    |
	| NEO-SC-BR-10004 | Stream context title length can not be greater than 128           | sssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss |                                                                                                                                                                                                                                                                   | Conversation    |
	| NEO-SC-BR-10005 | Stream context description length can not be greater than 256     | Career                                                                                                                            | sssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss | Conversation    |
	| NEO-SC-BR-10006 | Stream context can not be created due to empty stream event types | Career                                                                                                                            |                                                                                                                                                                                                                                                                   |                 |
	
Scenario: Stream context can not be modified when stream event type is not found
	And Stream event type 'Conclusion' has been removed
	When I reprovide stream context 'Career path' with following properties
		| Title      | Description |
		| My Familiy |             |
	And I reprovide the following stream event types
		| StreamEventType |
		| Conclusion      |
	And I modify stream context 'Career path'
	Then I get error with code 'NEO-SC-BR-10007' and message 'Stream context can not be created due to invalid stream event type' from the system

Scenario: Stream context is not allowed to get modified with duplicated stream event type
	When I reprovide stream context 'Career path' with following properties
		| Title      | Description |
		| My Familiy |             |
	And I reprovide the following stream event types
		| StreamEventType |
		| Conversation    |
		| Conversation    |
	And I modify stream context 'Career path'
	Then I get error with code 'NEO-SC-BR-10008' and message 'There are duplicated stream event types' from the system

		#TODO: comming soon
Scenario: Stream context stream event types is not allowed to get modified when an stream event is appended in