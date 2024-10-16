import React from 'react';
import './FlipCard.css';
import { useParams } from 'react-router-dom';
import { ErrorTypes } from './errorEnums';

enum State{
    UNANSWERED,
    ANSWERED,
    INCORRECT
}
interface FlipCardData {
    Id: number;
    Question: string;
    Concept: string;
    Mnemonic: string;
    State: State;
}

const FlipCard: React.FC = () => {
    const { title } = useParams<{ title: string }>();
    const [cards, setCards] = React.useState<FlipCardData[]>([]);
    const [loading, setLoading] = React.useState<boolean>(true);
    const [error, setError] = React.useState<string | null>(null);
    const [inputValue, setInputValue] = React.useState('');

    const flipAnsweredCard = (event: React.ChangeEvent<HTMLInputElement>) => {
        setInputValue(event.target.value);

        const updatedCards = [...cards];

        updatedCards.forEach(card => {
            if (card.Concept.toLowerCase() === event.target.value.toLowerCase()) {
                card.State = State.ANSWERED;
                handleFlip(card.Id);
            }
        });
        setCards(updatedCards);
    };

    const handleFlip = (id: number) => {
        const flipCard = document.getElementById(`flipCard-${id}`);
        if (flipCard) {
            flipCard.classList.toggle('flipped');
        }
    };

    React.useEffect(() => {
        const fetchCards = async () => {
            if (!title) {
                setError(ErrorTypes.TITLE);
                setLoading(false);
                return;
            }
            try {
                const response = await fetch(`https://localhost:7146/api/Home/${title}`);

                if (!response.ok) {
                    throw new Error(ErrorTypes.NETWORK);
                }

                const data = await response.json();

                if (data && Array.isArray(data._flipcards_list)) {
                    setCards(data._flipcards_list);
                } else {
                    setError(ErrorTypes.FORMAT + JSON.stringify(data));
                }
            } catch (error) {
                setError(ErrorTypes.CARDS + (error as Error).message);
            } finally {
                setLoading(false);
            }
        };

        fetchCards();
    }, [title]);

    if (loading) {
        return <div>Loading...</div>;
    }

    if (error) {
        return <div>{error}</div>;
    }

    if (!Array.isArray(cards) || cards.length === 0) {
        return <div>No cards available</div>;
    }

    return (
        <div>
            <div className="container">
                {cards.map((card) => (
                    <div
                        className="flip-card"
                        key={card.Id}
                        id={`flipCard-${card.Id}`}
                        onClick={() => handleFlip(card.Id)}
                    >
                        <div className="flip-card-inner">
                            <div className="flip-card-front">
                                <p className="card-question">{card.Question}</p>
                                <h2>{card.Mnemonic}</h2>
                            </div>
                            <div className="flip-card-back">
                                <h2>{card.Concept}</h2>
                            </div>
                        </div>
                    </div>
                ))}
            </div>
            <div className="input-container">
                <input
                    type="text"
                    placeholder="Type any concept..."
                    value={inputValue}
                    onChange={flipAnsweredCard}
                />
            </div>
        </div>
    );

};

export default FlipCard;