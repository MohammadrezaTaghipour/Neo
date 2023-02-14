Feature: Appending stream event

In order to registering and tracking life strams online
As me
I want to append an stream event

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
		| Title       | Description                                             |
		| Career Path | this stream context includes all career-related stuffs. |
	And With following stream event types
		| StreamEventType |
		| Conversation    |
	And There is a defined stream context 'Career Path'
	And There is a defined life stream with following properties
		| Title                  | Description                         |
		| Friendship with Souzan | Our friendship started at July 2022 |

Scenario: Stream event gets appended with its valid properties
	When I provide a new stream event with following properties
		| LifeStream             | StreamContext | StreamEventType |
		| Friendship with Souzan | Career Path   | Conversation    |
	And With the following stream event metadata
		| Key         | Value                                                              |
		| Description | We talked about the importance of Design Patterns in code quality. |
		| HappenedOn  | 2022-01-01                                                         |
	And I append stream event 'Conversation'
	Then I can find stream event 'Conversation' with above properties

Scenario Outline: Stream event is not allowed to get appended with invalid properties
	When I provide a new stream event with following properties
		| LifeStream   | StreamContext   | StreamEventType   |
		| <lifeStream> | <streamContext> | <streamEventType> |
	And With the following stream event metadata
		| Key        | Value      |
		| <key>      | <value>    |
		| HappenedOn | 2022-01-01 |
	And I append stream event '<streamEventType>'
	Then I get error with code '<errorCode>' and message '<errorMessage>' from the system

Examples:
	| errorCode       | errorMessage                             | lifeStream             | streamContext | streamEventType | key         | value      |
	| NEO-SE-BR-10002 | Life stream is required                  |                        | Career Path   | Conversation    | Description | we had fun |
	| NEO-SE-BR-10003 | Stream context is required               | Friendship with Souzan |               | Conversation    | Description | we had fun |
	| NEO-SE-BR-10004 | Stream event type is required            | Friendship with Souzan | Career Path   |                 | Description | we had fun |
	| NEO-SE-BR-10005 | Stream event metadata values is required | Friendship with Souzan | Career Path   | Conversation    | Description |            |

Scenario: Stream event can not be appended when life stream is not found
	Given Life stream 'Friendship with Souzan' has been removed
	When I provide a new stream event with following properties
		| LifeStream             | StreamContext | StreamEventType |
		| Friendship with Souzan | Career Path   | Conversation    |
	And With the following stream event metadata
		| Key         | Value                                                              |
		| Description | We talked about the importance of Design Patterns in code quality. |
		| HappenedOn  | 2022-01-01                                                         |
	And I append stream event 'Conversation'
	Then I get error with code 'NEO-SE-BR-10006' and message 'Stream event can not be appended due to invalid life stream' from the system

Scenario: Stream event can not be appended when stream context is not found
	Given Stream context 'Career Path' has been removed
	When I provide a new stream event with following properties
		| LifeStream             | StreamContext | StreamEventType |
		| Friendship with Souzan | Career Path   | Conversation    |
	And With the following stream event metadata
		| Key         | Value                                                              |
		| Description | We talked about the importance of Design Patterns in code quality. |
		| HappenedOn  | 2022-01-01                                                         |
	And I append stream event 'Conversation'
	Then I get error with code 'NEO-SE-BR-10007' and message 'Stream event can not be appended due to invalid stream context' from the system

Scenario: Stream event can not be appended when stream event type is not found
	Given Stream event type 'Conclusion' has been removed
	When I provide a new stream event with following properties
		| LifeStream             | StreamContext | StreamEventType |
		| Friendship with Souzan | Career Path   | Conclusion      |
	And With the following stream event metadata
		| Key         | Value                                                              |
		| Description | We talked about the importance of Design Patterns in code quality. |
		| HappenedOn  | 2022-01-01                                                         |
	And I append stream event 'Conclusion'
	Then I get error with code 'NEO-SE-BR-10008' and message 'Stream event can not be appended due to invalid stream event type' from the system

Scenario: Stream event metadata is based on what has been configured in stream event type
	When I provide a new stream event with following properties
		| LifeStream             | StreamContext | StreamEventType |
		| Friendship with Souzan | Career Path   | Conversation    |
	And With the following stream event metadata
		| Key         | Value                                                              |
		| Description | We talked about the importance of Design Patterns in code quality. |
	And I append stream event 'Conversation'
	Then I get error with code 'NEO-SE-BR-10009' and message 'Stream event metadata should be provided based on what has been configured in stream event type' from the system

Scenario: Stream event metadata is based on what has been configured in stream event type(metadata key mismatch)
	When I provide a new stream event with following properties
		| LifeStream             | StreamContext | StreamEventType |
		| Friendship with Souzan | Career Path   | Conversation    |
	And With the following stream event metadata
		| Key         | Value                                                              |
		| Description | We talked about the importance of Design Patterns in code quality. |
		| Rate        | 8.2                                                                |
	And I append stream event 'Conversation'
	Then I get error with code 'NEO-SE-BR-10009' and message 'Stream event metadata should be provided based on what has been configured in stream event type' from the system
