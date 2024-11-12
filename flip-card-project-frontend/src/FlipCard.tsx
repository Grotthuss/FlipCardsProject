import React from 'react';
import './FlipCard.css';
import AddFlipCard from "./AddFlipcard";
import { useParams, useNavigate } from 'react-router-dom';
import { Errors } from "./errorEnums";

interface FlipCardData {
    id: number;
    question: string;
    concept: string;
    mnemonic: string;
    state: {
        _state: number;
    };
}

const FlipCard: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const [cards, setCards] = React.useState<FlipCardData[]>([]);
    const [loading, setLoading] = React.useState<boolean>(true);
    const [error, setError] = React.useState<string | null>(null);

    const shuffleCards = async () => {
        try {
            const response = await fetch(`https://localhost:44372/api/Home/${id}/ShuffleCards`, {
                method: 'GET',
            });

            if (!response.ok) {
                throw new Error('Failed to shuffle cards.');
            }

            const data = await response.json();
            if (data && Array.isArray(data.flipcardsList)) {
                setCards(data.flipcardsList);
            } else {
                setError(Errors.FORMAT + JSON.stringify(data));
            }
        } catch (error) {
            setError(`Shuffle Error: ${(error as Error).message}`);
        }
    };

    const handleFlip = (index: number) => {
        const flipCard = document.getElementById(`flipCard-${index}`);
        if (flipCard) {
            flipCard.classList.toggle('flipped');
        }
    };

    const fetchCards = async () => {
        if (!id) {
            setError(Errors.TITLE);
            setLoading(false);
            return;
        }
        try {
            const response = await fetch(`https://localhost:44372/api/Home/${id}/GetCardSet`);
            if (!response.ok) {
                throw new Error(Errors.NETWORK);
            }
            const data = await response.json();

            if (data && Array.isArray(data.flipcardsList)) {
                setCards(data.flipcardsList);
            } else {
                setError(Errors.FORMAT + JSON.stringify(data));
            }
        } catch (error) {
            setError(Errors.CARDS + (error as Error).message);
        } finally {
            setLoading(false);
        }
    };

    React.useEffect(() => {
        fetchCards();
    }, [id]);

    const handleAddFlipCard = async (question: string, concept: string, mnemonic: string) => {
        const newCard: Omit<FlipCardData, 'id'> = {
            question: question,
            concept: concept,
            mnemonic: mnemonic,
            state: {
                _state: 0
            }
        };

        try {
            const response = await fetch(`https://localhost:44372/api/Home/${id}/CreateCard`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(newCard),
            });

            if (!response.ok) {
                const errorMessage = await response.text();
                console.error("Error response:", errorMessage);
                throw new Error(`${Errors.CARD} Server response: ${errorMessage}`);
            }

            await fetchCards();
            setError(null);

        } catch (err) {
            setError(Errors.CARD + (err as Error).message);
        }
    };

    const goToQuiz = () => {
        navigate(`/quizcard/${id}`, { state: { cards } });
    };

    if (loading) {
        return <div>Loading...</div>;
    }

    if (error) {
        return <div>{error}</div>;
    }

    return (
        <div className="flip-card-page">
            <div className="cards-container">
                {cards.length > 0 ? (
                    cards.map((card, index) => (
                        <div
                            className="flip-card"
                            key={card.id}
                            id={`flipCard-${index}`}
                            onClick={() => handleFlip(index)}
                        >
                            <div className="flip-card-inner">
                                <div className="flip-card-front">
                                    <p>{card.question}</p>
                                    <h2>{card.mnemonic}</h2>
                                </div>
                                <div className="flip-card-back">
                                    <h2>{card.concept}</h2>
                                </div>
                            </div>
                        </div>
                    ))
                ) : (
                    <div>No cards available</div>
                )}
            </div>
            <AddFlipCard onAddFlipCard={handleAddFlipCard} />

            <button onClick={shuffleCards} className="shuffle-button">Shuffle Cards</button>

            <button onClick={goToQuiz}>Go to Quiz</button>
        </div>
    );
};

export default FlipCard;
