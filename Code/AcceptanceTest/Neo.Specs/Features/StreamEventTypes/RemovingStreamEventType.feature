Feature: Removing stream event type

    In order to registering and tracking life strams online
    As me
    I want to configure Stream event type

Scenario Outline: Stream event type gets removed when it is not used by any life stream
	Given There are some provided stream event type metadata with following properties
		| Title     |
		| Init Date |
	And There is a defined stream event type with following properties
		| Title | Metadata |
		| Init  |          |
	When I remove stream event type 'Init'
	Then I can not find stream event type 'Init' with above properties

Scenario: Stream event type is not allowed to get removed when it is used by a stream context
	Given There are some provided stream event type metadata with following properties
		| Title      |
		| HappenedOn |
	And There are some defined stream event types with following properties
		| Title        | Metadata |
		| Conversation |          |
	And there is a provided stream context with following properties
		| Title       | Description                                                                                            |
		| Career path | this stream context includes all career-related stuffs such as my colleges and companies I worked for. |
	And With following stream event types
		| StreamEventType |
		| Conversation    |
	And There is a defined stream context 'Career path'
	When I remove stream event type 'Conversation'
	Then I get error with code 'NEO-SET-BR-10010' and message 'Stream event type can not be removed due to its usage' from the system
