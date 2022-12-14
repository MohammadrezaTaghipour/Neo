Feature: Removing stream context

In order to registering and tracking life strams online
As me
I want to configure stream contexts

Scenario Outline: Stream context gets removed when no any events has registered for it
	Given There are some defined stream event type with following properties
		| Title        | Metadata |
		| Conversation |          |
		| Conclusion   |          |
	And there is a provided stream context with following properties
		| Title       | Description |
		| Career path |             |
	And With following stream event type
		| StreamEventTypes |
		| Conversation     |
	And There is a defined stream context 'Career path'
	When I Remove stream context 'Career path'
	Then I can not find stream context 'Career path' with above properties