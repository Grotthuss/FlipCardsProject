import React from 'react';
import { Link } from 'react-router-dom';
import './CardSetSelection.css';
import { ErrorTypes } from './errorEnums';

enum State{
    UNANSWERED,
    ANSWERED,
    INCORRECT
}
interface CardAttribute {
    Id: number;
    Question: string;
    Concept: string;
    Mnemonic: string;
    State: State;
}

interface CardSet {
    _set_name: string;
    _flipcards_list: CardAttribute[];
}

const CardSetSelection: React.FC = () => {
    const [cardSets, setCardSets] = React.useState<CardSet[]>([]);
    const [loading, setLoading] = React.useState<boolean>(true);
    const [error, setError] = React.useState<string | null>(null);

    React.useEffect(() => {
        const fetchCardSets = async () => {
            try {
                const response = await fetch('https://localhost:7146/api/Home');
                if (!response.ok) {
                    throw new Error(ErrorTypes.NETWORK);
                }
                const data = await response.json();
                setCardSets(data);
            } catch (error) {
                setError(ErrorTypes.SETS + (error as Error).message);
            } finally {
                setLoading(false);
            }
        };

        fetchCardSets();
    }, []);

    if (loading) {
        return <div>Loading...</div>;
    }

    if (error) {
        return <div>{error}</div>;
    }

    return (
        <div className="card-set-selection">
            {Array.isArray(cardSets) && cardSets.length > 0 ? (
                cardSets.map((cardSet, index) => (
                    <div key={index} className="card-set">
                        <Link to={`/card-set/${cardSet._set_name}`}>
                            <div className="card">
                                <h2>{cardSet._set_name}</h2>
                            </div>
                        </Link>
                    </div>
                ))
            ) : (
                <div>No card sets available</div>
            )}
        </div>
    );
};

export default CardSetSelection;
