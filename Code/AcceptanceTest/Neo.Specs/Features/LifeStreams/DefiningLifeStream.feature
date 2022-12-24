﻿Feature: Defining life Stream

In order to registering and tracking life strams online
As me
I want to configure life stream 

Scenario Outline: Life stream gets defined with its valid properties
	When I define a new life stream with following properties
		| Title   | Description   |
		| <title> | <description> |
	Then I can find life stream '<title>' with above properties

Examples:
	| title                                                                                                                            | description                                                                                                                                                                                                                                                      |
	| Friendship with Souzan                                                                                                           | our friendship started at July 2022                                                                                                                                                                                                                              |
	| ssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss |                                                                                                                                                                                                                                                                  |
	| X                                                                                                                                | ssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss |

Scenario Outline: Life stream is not allowed to get defined with invalid properties
	When I define a new life stream with following properties
		| Title   | Description   |
		| <title> | <description> |
	Then I get error with code '<errorCode>' and message '<errorMessage>' from the system

Examples:
	| errorCode       | errorMessage                                               | title                                                                                                                             | description                                                                                                                                                                                                                                                       |
	| NEO-LS-BR-10002 | Life stream title is required                              |                                                                                                                                   |                                                                                                                                                                                                                                                                   |
	| NEO-LS-BR-10003 | Life stream title can not include special characters       | $Friendship with Souzan                                                                                                           |                                                                                                                                                                                                                                                                   |
	| NEO-LS-BR-10003 | Life stream title can not include special characters       | &Friendship with Souzan                                                                                                           |                                                                                                                                                                                                                                                                   |
	| NEO-LS-BR-10003 | Life stream title can not include special characters       | @Friendship with Souzan                                                                                                           |                                                                                                                                                                                                                                                                   |
	| NEO-LS-BR-10003 | Life stream title can not include special characters       | #Friendship with Souzan                                                                                                           |                                                                                                                                                                                                                                                                   |
	| NEO-LS-BR-10003 | Life stream title can not include special characters       | !Friendship with Souzan                                                                                                           |                                                                                                                                                                                                                                                                   |
	| NEO-LS-BR-10003 | Life stream title can not include special characters       | [Friendship with Souzan                                                                                                           |                                                                                                                                                                                                                                                                   |
	| NEO-LS-BR-10003 | Life stream title can not include special characters       | Friendship with Souzan]                                                                                                           |                                                                                                                                                                                                                                                                   |
	| NEO-LS-BR-10003 | Life stream title can not include special characters       | (Friendship with Souzan                                                                                                           |                                                                                                                                                                                                                                                                   |
	| NEO-LS-BR-10003 | Life stream title can not include special characters       | Friendship with Souzan)                                                                                                           |                                                                                                                                                                                                                                                                   |
	| NEO-LS-BR-10004 | Life stream title length can not be greater than 128       | sssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss |                                                                                                                                                                                                                                                                   |
	| NEO-LS-BR-10005 | Life stream description length can not be greater than 256 | X                                                                                                                                 | sssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss |
